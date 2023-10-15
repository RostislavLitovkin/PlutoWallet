using Substrate.NetApi.Generated.Model.orml_tokens;
using Substrate.NetApi.Generated.Model.pallet_asset_registry.types;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.pallet_omnipool.types;
using Substrate.NetApi.Model.Types.Primitive;
using static Substrate.NetApi.Model.Meta.Storage;
using System.Numerics;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model.HydraDX
{
	public class OmnipoolModel
	{
		public static async Task<List<OmnipoolLiquidityInfo>> GetOmnipoolLiquidityAmount(SubstrateClientExt client)
		{
            // Get all position keys
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            U128 collectionId = new U128();
            collectionId.Create(1337);

            var keyBytes = RequestGenerator.GetStorageKeyBytesHash("Uniques", "Account");

            byte[] prefix = keyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode()))
                .Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, collectionId.Encode())).ToArray();

            string prefixString = Utils.Bytes2HexString(prefix);

            byte[] startKey = null;

            List<U128> positionIds;

            var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, CancellationToken.None);

            if (keysPaged == null || !keysPaged.Any())
            {
                return new List<OmnipoolLiquidityInfo>();
            }
            else
            {
                positionIds = keysPaged.Select(p => HashModel.GetU128FromBlake2_128Concat(p.ToString().Substring(226))).ToList();
            }

            List<OmnipoolLiquidityInfo> result = new List<OmnipoolLiquidityInfo>();

            foreach (U128 positionId in positionIds)
            {
                Position position = await client.OmnipoolStorage.Positions(positionId, CancellationToken.None);

                AssetMetadata assetMetadata = await client.AssetRegistryStorage.AssetMetadataMap(position.AssetId, CancellationToken.None);

                var omnipoolAccount = new AccountId32();
                omnipoolAccount.Create(Utils.GetPublicKeyFrom(Constants.HydraDX.OmnipoolAddress));

                var omnipoolTokensKey = new Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>();
                omnipoolTokensKey.Create(omnipoolAccount, position.AssetId);

                AccountData omnipoolTokens = await client.TokensStorage.Accounts(omnipoolTokensKey, CancellationToken.None);

                Console.WriteLine(omnipoolTokens.Free);

                AssetState omnipoolAssetState = await client.OmnipoolStorage.Assets(position.AssetId, CancellationToken.None);

                Console.WriteLine(omnipoolAssetState.Shares);

                double liquidity = (double)CalculateRemoveLiquidityStateChanges(
                        omnipoolAssetState,
                        omnipoolTokens,
                        position.Shares,
                        position,
                        omnipoolAssetState.Shares,
                        0
                    ).Asset.DeltaReserve / Math.Pow(10, assetMetadata.Decimals.Value);
                double initialLiquidity = (double)position.Amount.Value / Math.Pow(10, assetMetadata.Decimals.Value);

                result.Add(new OmnipoolLiquidityInfo
                {
                    Amount = liquidity,
                    Symbol = Model.ToStringModel.VecU8ToString(assetMetadata.Symbol.Value.Value),
                    InitialAmount = initialLiquidity,
                });
            }

            return result;
		}

        // Source: https://github.com/galacticcouncil/HydraDX-node/blob/a578bc00023a2b0acce4034602b5a5dfb38045d5/math/src/omnipool/math.rs#L397
        public static LiquidityStateChange CalculateRemoveLiquidityStateChanges(
            AssetState asset_state,
            AccountData omnipoolTokens,
            U128 shares_removed_u128,
            Position position,
            //imbalance: I129<Balance>, // Add later
            U128 total_hub_reserve,
            decimal withdrawal_fee
        ) {
	        BigInteger current_shares = asset_state.Shares.Value;
            BigInteger current_reserve = omnipoolTokens.Free.Value;
            BigInteger current_hub_reserve = asset_state.HubReserve.Value;
            BigInteger position_amount = position.Amount.Value;
            BigInteger position_shares = position.Shares.Value;
            BigInteger shares_removed = shares_removed_u128.Value;

            decimal current_price = ((decimal)current_reserve) / (decimal)current_hub_reserve;
            decimal position_price = ((decimal)((U128)position.Price.Value[0]).Value) / (decimal)((U128)position.Price.Value[1]).Value;

            // This might be wrong
            BigInteger p_x_r = (BigInteger)(position_price * (decimal)current_reserve) + 1;// ?).checked_add(U256::one()) ?;

            // Protocol shares update
            BigInteger delta_b = 0;
            if (current_price < position_price) {
		        BigInteger numer = (p_x_r - current_hub_reserve) * shares_removed;
                BigInteger denom = p_x_r + current_hub_reserve;
                delta_b = (numer / denom) + 1; // round up
	        }

            BigInteger delta_shares = shares_removed - delta_b;

            BigInteger delta_reserve = (current_reserve * delta_shares) / current_shares;
            BigInteger delta_hub_reserve = (delta_reserve * current_hub_reserve) / current_reserve;

            BigInteger delta_position_amount = shares_removed * position_amount / position_shares;

            BigInteger hub_transferred = 0;

            if (current_price > position_price) {
		        // LP receives some hub asset

		        // delta_q_a = -pi * ( 2pi / (pi + pa) * delta_s_a / Si * Ri + delta_r_a )
		        // note: delta_s_a is < 0

		        BigInteger sub = current_hub_reserve - p_x_r;
                BigInteger sum = current_hub_reserve + p_x_r;
                BigInteger div1 = (current_hub_reserve * sub) / sum;

                hub_transferred = (div1 * delta_shares) / current_shares;
	        }

            decimal fee_complement = 1 - withdrawal_fee;

            // Apply withdrawal fee
            delta_reserve = (BigInteger)(fee_complement * (decimal)delta_reserve);
            delta_hub_reserve = (BigInteger)(fee_complement * (decimal)delta_hub_reserve);
            hub_transferred = (BigInteger)(fee_complement * (decimal)hub_transferred);

            // Might be useful in the future
            //let delta_imbalance = calculate_delta_imbalance(delta_hub_reserve, imbalance, total_hub_reserve) ?;

            return new LiquidityStateChange
            {
                Asset = new AssetStateChange
                {
                    DeltaReserve = delta_reserve,
                    DeltaHubReserve = delta_hub_reserve,
                    DeltaShares = delta_shares,
                    DeltaProtocolShares = delta_b,
                },
                //DeltaImbalance = null,
                LPHubAmount = hub_transferred,
                DeltaPositionReserve = delta_position_amount,
                DeltaPositionShares = shares_removed,
            };
        }
	}

    public class OmnipoolLiquidityInfo
    {
        public double Amount { get; set; }
        public string Symbol { get; set; }
        public double InitialAmount { get; set; }
    }

    public class LiquidityStateChange
    {
        public AssetStateChange Asset { get; set; }
        public BigInteger DeltaImbalance { get; set; }
        public BigInteger LPHubAmount { get; set; }
        public BigInteger DeltaPositionReserve { get; set; }
        public BigInteger DeltaPositionShares { get; set; }
    }

    public class AssetStateChange
    {
        public BigInteger DeltaReserve { get; set; }
        public BigInteger DeltaHubReserve { get; set; }
        public BigInteger DeltaShares { get; set; }
        public BigInteger DeltaProtocolShares { get; set; }
    }
}

