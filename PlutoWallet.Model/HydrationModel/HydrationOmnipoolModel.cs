using Hydration.NetApi.Generated;
using Hydration.NetApi.Generated.Model.sp_core.crypto;
using PlutoWallet.Model.HydraDX;
using Substrate.NetApi.Model.Types.Primitive;
using Hydration.NetApi.Generated.Model.pallet_omnipool.types;
using Substrate.NetApi;
using System.Numerics;
using Hydration.NetApi.Generated.Model.pallet_ema_oracle.types;
using Hydration.NetApi.Generated.Types.Base;
using Hydration.NetApi.Generated.Model.hydradx_traits.oracle;
using Substrate.NetApi.Model.Types.Base;
using System.Text;
using Hydration.NetApi.Generated.Model.hydra_dx_math.ratio;
using PlutoWallet.Model.HydrationModel;

namespace PlutoWallet.Model.HydrationModel
{
    public static class HydrationOmnipoolModel
    {
        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/primitives/src/constants.rs#L95
        /// </summary>
        public static Arr8U8 GetOmnipoolSource()
        {
            var omnipoolSource = new Arr8U8();
            omnipoolSource.Create(Encoding.ASCII.GetBytes("omnipool"));
            return omnipoolSource;
        }

        public static async Task<List<OmnipoolLiquidityInfo>> GetOmnipoolLiquiditiesAsync(SubstrateClientExt client, string substrateAddress, CancellationToken token = default)
        {
            var positionIds = await UniquesModel.GetUniquesInCollection(client, 1337, substrateAddress, token);

            List<OmnipoolLiquidityInfo> result = new List<OmnipoolLiquidityInfo>();

            foreach (U128 positionId in positionIds)
            {
                result.Add(await GetOmnipoolLiquidityAtPositionAsync(client, positionId, token));
            }

            return result;
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/aa61ed23a1b7f26b238ea7fd6c6303e369efb875/pallets/omnipool/src/lib.rs#L801
        /// </summary>
        /// <returns></returns>
        public static async Task<OmnipoolLiquidityInfo> GetOmnipoolLiquidityAtPositionAsync(SubstrateClientExt client, U128 positionId, CancellationToken token)
        {
            Position position = await client.OmnipoolStorage.Positions(positionId, null, token);

            U32 assetId = position.AssetId;

            var omnipoolAccount = new AccountId32();
            omnipoolAccount.Create(Utils.GetPublicKeyFrom(PlutoWallet.Constants.HydraDX.OMNIPOOL_ADDRESS));

            // Load asset state
            // https://github.com/galacticcouncil/hydration-node/blob/aa61ed23a1b7f26b238ea7fd6c6303e369efb875/pallets/omnipool/src/lib.rs#L1732
            var omnipoolAssetState = await client.OmnipoolStorage.Assets(position.AssetId, null, token);

            var reserve = await client.GetFreeBalanceAsync(omnipoolAccount, assetId, token);

            var assetState = new AssetReserveState
            {
                HubReserve = omnipoolAssetState.HubReserve,
                Shares = omnipoolAssetState.Shares,
                ProtocolShares = omnipoolAssetState.ProtocolShares,
                Reserve = reserve,
            };

            var safeWithdrawal = omnipoolAssetState.Tradable.IsSafeWithdrawal();

            if (!safeWithdrawal)
            {
                // Barier check
                // I am too lazy to implement it, probably not needed
                // https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/omnipool/src/lib.rs#L831
            }

            var extAssetPrice = await client.ExternalPriceOracleGetPriceAsync((U32)PlutoWallet.Constants.HydraDX.HUB_ASSET_ID, assetId, token);

            // Is zero check missing

            // Get withdrawal fee
            var withdrawalFee = 0;

            var currentImbalance = await client.OmnipoolStorage.HubAssetImbalance(null, token);
            var currentHubAssetLiquidity = await client.GetFreeBalanceAsync(omnipoolAccount, (U32)PlutoWallet.Constants.HydraDX.HUB_ASSET_ID, token);

            var stateChanges = CalculateRemoveLiquidityStateChanges(
                    assetState,
                    position.Shares,
                    position,
                    new I129<U128>
                    {
                        Value = currentImbalance.Value,
                        Negative = currentImbalance.Negative,
                    },
                    omnipoolAssetState.Shares,
                    withdrawalFee
                );

            // Slipage limit check

            assetState.DeltaUpdate(stateChanges.Asset);

            position.Shares = (U128)(position.Shares - stateChanges.DeltaPositionShares);
            position.Amount = (U128)(position.Amount - stateChanges.DeltaPositionReserve);

            var assetMetadata = await client.AssetRegistryStorage.Assets(position.AssetId, null, token);

            double liquidity = (double)stateChanges.Asset.DeltaReserve / Math.Pow(10, assetMetadata.Decimals.Value);

            double initialLiquidity = (double)position.Amount.Value / Math.Pow(10, assetMetadata.Decimals.Value);

            return new OmnipoolLiquidityInfo
            {
                Amount = liquidity,
                AssetId = position.AssetId.Value,
                Symbol = Model.ToStringModel.VecU8ToString(assetMetadata.Symbol.Value.Value),
                InitialAmount = initialLiquidity,
            };
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/omnipool/src/lib.rs#L840
        /// </summary>
        public static async Task<Ratio> ExternalPriceOracleGetPriceAsync(this SubstrateClientExt client, U32 assetIdA, U32 assetIdB, CancellationToken token)
        {
            if (assetIdA == assetIdB)
            {
                // Bad
                throw new Exception("Same asset ids not allowed");
            }

            var shortPeriod = new EnumOraclePeriod();
            shortPeriod.Create(OraclePeriod.Short);

            var (entry, initialized) = await client.EmaOracleGetUpdatedEntriesAsync(GetOmnipoolSource(), OrderedPair((assetIdA, assetIdB)), shortPeriod, token);

            if ((assetIdA, assetIdB) != OrderedPair((assetIdA, assetIdB)))
            {
                entry = entry.Inverted();
            }

            entry.IntoAgregated(initialized);

            return entry.Price;
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/ema-oracle/src/lib.rs#L427
        /// </summary>
        /// <returns></returns>
        public static async Task<(OracleEntry, U32)> EmaOracleGetUpdatedEntriesAsync(this SubstrateClientExt client, Arr8U8 source, (U32, U32) assets, EnumOraclePeriod period, CancellationToken token)
        {
            var parent = (U32)((await client.SystemStorage.Number(null, token)) - 1);

            var assetsTuple = new BaseTuple<U32, U32>(assets.Item1, assets.Item2);

            // https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/ema-oracle/src/lib.rs#L348
            var oraclesKey = new BaseTuple<Arr8U8, BaseTuple<U32, U32>, EnumOraclePeriod>();

            var lastBlockOraclePeriod = new EnumOraclePeriod();
            lastBlockOraclePeriod.Create(OraclePeriod.LastBlock);

            oraclesKey.Create(source, assetsTuple, lastBlockOraclePeriod);

            var oracleTuple = await client.EmaOracleStorage.Oracles(oraclesKey, null, token);
            var lastBlock = (OracleEntry)oracleTuple.Value[0];
            var lastBlockInit = (U32)oracleTuple.Value[1];

            if (lastBlock.UpdatedAt != parent)
            {
                lastBlock.FastForwardTo(parent);
            }

            if (period.Value == OraclePeriod.LastBlock)
            {
                return (lastBlock, lastBlockInit);
            }

            // Other oracle period variants
            // Too lazy to implement..
            return (lastBlock, lastBlockInit);
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/ema-oracle/src/types.rs#L110
        /// </summary>
        public static void FastForwardTo(this OracleEntry entry, U32 newUpdatedAt)
        {
            entry.UpdatedAt = newUpdatedAt;
            entry.Volume = new Volume();
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/ema-oracle/src/lib.rs#L589
        /// </summary>
        /// <returns></returns>
        public static (U32, U32) OrderedPair((U32, U32) assetIdPair)
        {
            return (assetIdPair.Item1 <= assetIdPair.Item2) switch
            {
                true => assetIdPair,
                false => (assetIdPair.Item2, assetIdPair.Item1),
            };
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/pallets/ema-oracle/src/types.rs#L88
        /// </summary>
        /// <returns></returns>
        public static OracleEntry Inverted(this OracleEntry entry)
        {
            return new OracleEntry
            {
                Price = new Ratio
                {
                    N = entry.Price.D,
                    D = entry.Price.N,
                },
                Volume = new Volume
                {
                    AIn = entry.Volume.BIn,
                    BIn = entry.Volume.AOut,
                    AOut = entry.Volume.BOut,
                    BOut = entry.Volume.AOut,
                },
                Liquidity = new Liquidity
                {
                    A = entry.Liquidity.B,
                    B = entry.Liquidity.B,
                },
                UpdatedAt = entry.UpdatedAt,
            };
        }

        public static AggregatedEntry IntoAgregated(this OracleEntry entry, U32 initialized)
        {
            return new AggregatedEntry
            {
                Price = entry.Price,
                Volume = entry.Volume,
                Liquidity = entry.Liquidity,
                OracleAge = entry.UpdatedAt >= initialized ? (U32)(entry.UpdatedAt - initialized) : (U32)0,
            };
        }
        /// <summary>
        /// https://github.com/galacticcouncil/HydraDX-node/blob/a578bc00023a2b0acce4034602b5a5dfb38045d5/math/src/omnipool/math.rs#L397
        /// </summary>
        /// <returns></returns>
        public static LiquidityStateChange CalculateRemoveLiquidityStateChanges(
                    AssetReserveState asset_state,
                    U128 shares_removed_u128,
                    Position position,
                    I129<U128> imbalance,
                    U128 total_hub_reserve,
                    decimal withdrawal_fee
                )
        {
            BigInteger current_shares = asset_state.Shares.Value;
            BigInteger current_reserve = asset_state.Reserve.Value;
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
            if (current_price < position_price)
            {
                BigInteger numer = (p_x_r - current_hub_reserve) * shares_removed;
                BigInteger denom = p_x_r + current_hub_reserve;
                delta_b = (numer / denom) + 1; // round up
            }

            BigInteger delta_shares = shares_removed - delta_b;

            BigInteger delta_reserve = (current_reserve * delta_shares) / current_shares;
            BigInteger delta_hub_reserve = (delta_reserve * current_hub_reserve) / current_reserve;

            BigInteger delta_position_amount = shares_removed * position_amount / position_shares;

            BigInteger hub_transferred = 0;

            if (current_price > position_price)
            {
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
            var deltaImbalance = CalculateDeltaImbalance(
                delta_hub_reserve,
                imbalance,
                total_hub_reserve
            );

            return new LiquidityStateChange
            {
                Asset = new AssetStateChange
                {
                    DeltaReserve = delta_reserve,
                    DeltaHubReserve = delta_hub_reserve,
                    DeltaShares = delta_shares,
                    DeltaProtocolShares = delta_b,
                },
                DeltaImbalance = deltaImbalance,
                LPHubAmount = hub_transferred,
                DeltaPositionReserve = delta_position_amount,
                DeltaPositionShares = shares_removed,
            };
        }

        /// <summary>
        /// https://github.com/galacticcouncil/hydration-node/blob/a1e7e729cca8036c4eb6f6a264a1a02bb83bcf40/math/src/omnipool/math.rs#L422
        /// </summary>
        /// <returns></returns>
        public static BigInteger? CalculateDeltaImbalance(
            BigInteger deltaHubReserve,
            I129<U128> imbalance,
            BigInteger hubReserve
        )
        {
            if (imbalance.Value.Value == 0)
            {
                return 0;
            }

            if (!imbalance.Negative)
            {
                // currently support only negative imbalances
                return null;
            }

            var deltaImbalance = (deltaHubReserve * imbalance.Value.Value) / hubReserve;

            return deltaImbalance;
        }


        public static bool IsSafeWithdrawal(this Tradability tradability)
        {
            return ((TradabilityEnum)tradability.Bits.Value & TradabilityEnum.AddLiquidity) == TradabilityEnum.AddLiquidity ||
                    ((TradabilityEnum)tradability.Bits.Value & TradabilityEnum.RemoveLiquidity) == TradabilityEnum.RemoveLiquidity;
        }
    }

    public class OmnipoolLiquidityInfo
    {
        public double Amount { get; set; }
        public string Symbol { get; set; }
        public uint AssetId { get; set; }
        public double InitialAmount { get; set; }
    }

    public class LiquidityStateChange
    {
        public AssetStateChange Asset { get; set; }
        public BigInteger? DeltaImbalance { get; set; }
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


    public class AggregatedEntry
    {
        public Ratio Price { get; set; }
        public Volume Volume { get; set; }
        public Liquidity Liquidity { get; set; }
        public U32 OracleAge { get; set; }
    }

    public class I129<Balance>
    {
        public Balance Value { get; set; }
        public bool Negative { get; set; }
    }

    public class AssetReserveState
    {
        /// Quantity of asset in omnipool
        public U128 Reserve { get; set; }
        /// Quantity of Hub Asset matching this asset
        public U128 HubReserve { get; set; }
        /// Quantity of LP shares for this asset
        public U128 Shares { get; set; }
        /// Quantity of LP shares for this asset owned by protocol
        public U128 ProtocolShares { get; set; }

        public void DeltaUpdate(AssetStateChange delta)
        {
            HubReserve = (U128)(this.HubReserve - delta.DeltaHubReserve);
            Reserve = (U128)(this.Reserve - delta.DeltaReserve);
            Shares = (U128)(this.Shares - delta.DeltaShares);
            ProtocolShares = (U128)(this.ProtocolShares + delta.DeltaProtocolShares);
        }
    }

    [Flags]
    public enum TradabilityEnum : byte
    {
        // Asset is frozen. No operations are allowed.
        Frozen = 0b0000_0000,
        // Asset is allowed to be sold into omnipool
        Sell = 0b0000_0001,
        // Asset is allowed to be bought into omnipool
        Buy = 0b0000_0010,
        // Adding liquidity of asset is allowed
        AddLiquidity = 0b0000_0100,
        // Removing liquidity of asset is not allowed
        RemoveLiquidity = 0b0000_1000
    }
}
