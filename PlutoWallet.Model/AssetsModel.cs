using System;
using Substrate.NetApi.Generated.Model.pallet_assets.types;
using System.Numerics;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using static Substrate.NetApi.Model.Meta.Storage;
using Substrate.NetApi.Generated.Model.orml_tokens;
using PlutoWallet.Types;
using PlutoWallet.Constants;

namespace PlutoWallet.Model
{
    public class AssetsModel
    {
        private static bool doNotReload = false;

        public static List<Asset> Assets;

        public static double UsdSum = 0.0;

        public static async Task GetBalance(SubstrateClientExt[] groupClients, Endpoint[] groupEndpoints, string substrateAddress, bool darkIcon)
        {
            if (doNotReload)
            {
                return;
            }

            var tempAssets = new List<Asset>();

            double usdSumValue = 0;

            for (int i = 0; i < groupClients.Length; i++)
            {
                SubstrateClientExt client = groupClients[i];
                var endpoint = groupEndpoints[i];

                if (endpoint.ChainType != Constants.ChainType.Substrate)
                {
                    /*tempAssets.Add(new Asset
                    {
                        Amount = "Unsupported",
                        //Symbol = endpoint.Unit, // I think it looks better without it
                        //ChainIcon = endpoint.Icon,
                        //DarkChainIcon = endpoint.DarkIcon,
                        Endpoint = endpoint,
                        UsdValue = String.Format("{0:0.00}", 0) + " USD",
                    });*/

                    continue;
                }

                double amount = 0;

                try
                {
                    var accountInfo = await client.SystemStorage.Account(substrateAddress);

                    amount = (double)accountInfo.Data.Free.Value / Math.Pow(10, endpoint.Decimals);
                }
                catch
                {
                    // this usually means that nothing is saved for this account
                }

                // Calculate a real USD value
                {
                    double usdRatio = 0;

                    double usdValue = amount * usdRatio;

                    usdSumValue += usdValue;

                    tempAssets.Add(new Asset
                    {
                        Amount = amount,
                        Symbol = endpoint.Unit,
                        ChainIcon = endpoint.Icon,
                        DarkChainIcon = endpoint.DarkIcon,
                        Endpoint = endpoint,
                        Pallet = AssetPallet.Native,
                        AssetId = 0,
                        UsdValue = usdValue,
                        Decimals = endpoint.Decimals,
                    });
                }

                foreach ((BigInteger, AssetDetails, AssetMetadata, AssetAccount) asset in await client.AssetsStorage.GetAssetsMetadataAndAcountNextAsync(substrateAddress, 1000, CancellationToken.None))
                {
                    double usdRatio = 0;

                    double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value / Math.Pow(10, asset.Item3.Decimals.Value) : 0.0;

                    tempAssets.Add(new Asset
                    {
                        Amount = assetBalance,
                        Symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value.Value),
                        ChainIcon = endpoint.Icon,
                        DarkChainIcon = endpoint.DarkIcon,
                        Endpoint = endpoint,
                        Pallet = AssetPallet.Assets,
                        AssetId = asset.Item1,
                        UsdValue = assetBalance * usdRatio,
                        Decimals = asset.Item3.Decimals.Value,
                    });
                }

                foreach (TokenData tokenData in await GetTokensBalance(client, substrateAddress))
                {
                    double usdRatio = 0;

                    double assetBalance = (double)tokenData.AccountData.Free.Value / Math.Pow(10, tokenData.AssetMetadata.Decimals.Value);

                    tempAssets.Add(new Asset
                    {
                        Amount = assetBalance,
                        Symbol = Model.ToStringModel.VecU8ToString(tokenData.AssetMetadata.Symbol.Value.Value),
                        ChainIcon = endpoint.Icon,
                        DarkChainIcon = endpoint.DarkIcon,
                        Endpoint = endpoint,
                        Pallet = AssetPallet.Tokens,
                        AssetId = tokenData.AssetId,
                        UsdValue = assetBalance * usdRatio,
                        Decimals = tokenData.AssetMetadata.Decimals.Value,
                    });
                }
            }

            UsdSum = usdSumValue;

            Assets = tempAssets;

            if (Model.HydraDX.Sdk.Assets.Any())
            {
                GetUsdBalance();
            }
        }

        public static void GetUsdBalance()
        {
            double usdSumValue = 0.0;
            for(int i = 0; i < Assets.Count(); i++)
            {
                double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(Assets[i].Symbol);
                Assets[i].UsdValue = Assets[i].Amount * spotPrice;
                usdSumValue += Assets[i].UsdValue;
            }

            UsdSum = usdSumValue;
        }

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public async static Task<List<TokenData>> GetTokensBalance(SubstrateClientExt client, string substrateAddress)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            var tokensKeyBytes = RequestGenerator.GetStorageKeyBytesHash("Tokens", "Accounts");
            var assetRegistryKeyBytes = RequestGenerator.GetStorageKeyBytesHash("AssetRegistry", "AssetMetadataMap");

            byte[] prefix = tokensKeyBytes.Concat(HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
            byte[] startKey = null;

            List<string[]> storageTokensChanges = new List<string[]>();
            List<string[]> storageAssetRegistryChanges = new List<string[]>();
            List<string> storageKeys = new List<string>();

            int prefixLength = Utils.Bytes2HexString(prefix).Length;

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAtAsync(prefix, 1000, startKey, string.Empty, CancellationToken.None);

                if (keysPaged == null || !keysPaged.Any())
                {
                    break;
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, CancellationToken.None);
                    storageTokensChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));

                    var tar = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(Utils.Bytes2HexString(assetRegistryKeyBytes) + p.ToString().Substring(prefixLength))).ToList(), string.Empty, CancellationToken.None);
                    storageAssetRegistryChanges.AddRange(new List<string[]>(tar.ElementAt(0).Changes));

                    storageKeys.AddRange(keysPaged.Select(p => p.ToString().Substring(prefixLength)).ToList());

                    startKey = Utils.HexToByteArray(tt.ElementAt(0).Changes.Last()[0]);
                }
            }

            var resultList = new List<TokenData>();

            if (storageTokensChanges != null)
            {
                for (int i = 0; i < storageTokensChanges.Count(); i++)
                {
                    AccountData accountData = new AccountData();
                    accountData.Create(storageTokensChanges[i][1]);

                    Substrate.NetApi.Generated.Model.pallet_asset_registry.types.AssetMetadata assetMetadata = new Substrate.NetApi.Generated.Model.pallet_asset_registry.types.AssetMetadata();
                    assetMetadata.Create(storageAssetRegistryChanges[i][1]);

                    BigInteger assetId = Model.HashModel.GetBigIntegerFromTwox_64Concat(storageKeys[i]);

                    resultList.Add(new TokenData
                    {
                        AssetId = assetId,
                        AccountData = accountData,
                        AssetMetadata = assetMetadata,
                    });
                }
            }
            return resultList;
        }
    }

    public class TokenData {
        public BigInteger AssetId { get; set; }
        public AccountData AccountData { get; set; }
        public Substrate.NetApi.Generated.Model.pallet_asset_registry.types.AssetMetadata AssetMetadata { get; set; }
    }
}

