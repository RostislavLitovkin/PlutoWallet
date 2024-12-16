
using System;
using System.Numerics;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Polkadot.NetApi.Generated.Model.sp_core.crypto;
using PlutoWallet.Types;
using PlutoWallet.Constants;
using Bifrost.NetApi.Generated.Model.orml_tokens;
using Substrate.NetApi.Model.Types.Primitive;
using Bifrost.NetApi.Generated.Model.bifrost_asset_registry.pallet;

using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);
using AssetMetadata = PlutoWallet.Types.AssetMetadata;

namespace PlutoWallet.Model
{
    public class AssetsModel
    {
        private static bool doNotReload = false;

        public static double UsdSum = 0.0;

        public static Dictionary<AssetKey, Asset> AssetsDict = new System.Collections.Generic.Dictionary<AssetKey, Asset>();

        public static IEnumerable<Asset> GetAssetsWithSymbol(string symbol)
        {
            return AssetsDict.Values
                     .Where(asset => asset.Symbol.Equals(symbol, StringComparison.Ordinal));
        }


        public static async Task GetBalanceAsync(SubstrateClientExt client, string substrateAddress, CancellationToken token = default)
        {
            if (doNotReload)
            {
                return;
            }

            double usdSumValue = 0;

            var endpoint = client.Endpoint;

            if (endpoint.ChainType != PlutoWallet.Constants.ChainType.Substrate)
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

                return;
            }

            if (!await client.IsConnectedAsync())
            {
                return;
            }

            double amount = 0;

            try
            {
                var accountInfo = await GetNativeBalance(client.SubstrateClient, substrateAddress, token);

                amount = (double)accountInfo.Data.Free.Value / Math.Pow(10, endpoint.Decimals);
            }
            catch
            {
                // this usually means that nothing is saved for this account
            }

            // Calculate a real USD value
            {
                double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(endpoint.Unit);

                AssetsDict[(endpoint.Key, AssetPallet.Native, 0)] = new Asset
                {
                    Amount = amount,
                    Symbol = endpoint.Unit,
                    ChainIcon = endpoint.Icon,
                    DarkChainIcon = endpoint.DarkIcon,
                    Endpoint = endpoint,
                    Pallet = AssetPallet.Native,
                    AssetId = 0,
                    UsdValue = amount * spotPrice,
                    Decimals = endpoint.Decimals,
                };
            }

            try
            {
                
                foreach ((BigInteger, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount) asset in await GetPolkadotAssetHubAssetsAsync(client.SubstrateClient, substrateAddress, 1000, CancellationToken.None))
                {
                    var symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value);
                    double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(symbol);

                    double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value / Math.Pow(10, asset.Item3.Decimals.Value) : 0.0;

                    AssetsDict[(endpoint.Key, AssetPallet.Assets, asset.Item1)] = new Asset
                    {
                        Amount = assetBalance,
                        Symbol = symbol,
                        ChainIcon = endpoint.Icon,
                        DarkChainIcon = endpoint.DarkIcon,
                        Endpoint = endpoint,
                        Pallet = AssetPallet.Assets,
                        AssetId = asset.Item1,
                        UsdValue = assetBalance * spotPrice,
                        Decimals = asset.Item3.Decimals.Value,
                    };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                if (endpoint.Key != EndpointEnum.Bifrost)
                {
                    foreach (HydrationTokenData tokenData in await GetHydrationTokensBalance(client.SubstrateClient, substrateAddress, CancellationToken.None))
                    {
                        // Skip tokens without Symbol
                        if (!tokenData.AssetMetadata.Symbol.OptionFlag)
                        {
                            continue;
                        }

                        var symbol = tokenData.AssetMetadata.Symbol.OptionFlag ? Model.ToStringModel.VecU8ToString(tokenData.AssetMetadata.Symbol.Value.Value) : "";
                        double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(symbol);

                        double assetBalance = (double)tokenData.AccountData.Free.Value / Math.Pow(10, tokenData.AssetMetadata.Decimals.Value);

                        AssetsDict[(endpoint.Key, AssetPallet.Native, tokenData.AssetId)] = new Asset
                        {
                            Amount = assetBalance,
                            Symbol = symbol,
                            ChainIcon = endpoint.Icon,
                            DarkChainIcon = endpoint.DarkIcon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Tokens,
                            AssetId = tokenData.AssetId,
                            UsdValue = assetBalance * spotPrice,
                            Decimals = tokenData.AssetMetadata.Decimals.Value,
                        };
                    }
                }
                else
                {
                    foreach (BifrostTokenData tokenData in await GetBifrostTokensBalance(client.SubstrateClient, substrateAddress, CancellationToken.None))
                    {
                        var symbol = Model.ToStringModel.VecU8ToString(tokenData.AssetMetadata.Symbol.Value);
                        double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(symbol);

                        double assetBalance = (double)tokenData.AccountData.Free.Value / Math.Pow(10, tokenData.AssetMetadata.Decimals.Value);

                        AssetsDict[(endpoint.Key, AssetPallet.Native, tokenData.AssetId)] = new Asset
                        {
                            Amount = assetBalance,
                            Symbol = symbol,
                            ChainIcon = endpoint.Icon,
                            DarkChainIcon = endpoint.DarkIcon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Tokens,
                            AssetId = tokenData.AssetId,
                            UsdValue = assetBalance * spotPrice,
                            Decimals = tokenData.AssetMetadata.Decimals.Value,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            CalculateTotalUsdBalance();
        }

        public static void GetUsdBalance()
        {
            double usdSumValue = 0.0;

            foreach (var asset in AssetsDict.Values)
            {
                double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(asset.Symbol);
                asset.UsdValue = asset.Amount * spotPrice;
                usdSumValue += asset.UsdValue;
            }

            UsdSum = usdSumValue;
        }

        public static void CalculateTotalUsdBalance()
        {
            double usdSumValue = 0.0;

            foreach (var asset in AssetsDict.Values)
            {
                usdSumValue += asset.UsdValue;
            }

            UsdSum = usdSumValue;
        }

        /// <summary>
        /// Assumption: All chains use the same Balances pallet as Polkadot
        /// </summary>
        /// <param name="client"></param>
        /// <param name="substrateAddress"></param>
        /// <returns></returns>
        public static async Task<Polkadot.NetApi.Generated.Model.frame_system.AccountInfo> GetNativeBalance(SubstrateClient client, string substrateAddress, CancellationToken token)
        {
            var account = new Polkadot.NetApi.Generated.Model.sp_core.crypto.AccountId32();
            account.Create(Utils.GetPublicKeyFrom(substrateAddress));

            string parameters = Polkadot.NetApi.Generated.Storage.SystemStorage.AccountParams(account);
            return await client.GetStorageAsync<Polkadot.NetApi.Generated.Model.frame_system.AccountInfo>(parameters, null, token);
        }

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public static async Task<List<(BigInteger, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount)>> GetPolkadotAssetHubAssetsAsync(SubstrateClient client, string substrateAddress, uint page, CancellationToken token)
        {
            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            var resultList = new List<(BigInteger, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount)>();

            var detailsKeyPrefixBytes = RequestGenerator.GetStorageKeyBytesHash("Assets", "Asset");

            string detailsKeyPrefixBytesString = Utils.Bytes2HexString(detailsKeyPrefixBytes).ToLower();
            string metadataKeyPrefixBytesString = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Assets", "Metadata")).ToLower();
            string accountKeyPrefixBytesString = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash("Assets", "Account")).ToLower();

            var storageKeys = (await client.State.GetKeysPagedAsync(detailsKeyPrefixBytes, page, null, string.Empty, token))
                .Select(p => p.ToString().Replace(detailsKeyPrefixBytesString, ""));

            var assetDetailKeys = storageKeys.Select(p => Utils.HexToByteArray(detailsKeyPrefixBytesString + p.ToString().ToLower())).ToList();
            var assetMetadataKeys = storageKeys.Select(p => Utils.HexToByteArray(metadataKeyPrefixBytesString + p.ToString().ToLower())).ToList();
            var assetAccountKeys = storageKeys.Select(p => Utils.HexToByteArray(
                accountKeyPrefixBytesString +
                p.ToString().ToLower() +
                Utils.Bytes2HexString(HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, account32.Bytes), Utils.HexStringFormat.Pure).ToLower()
            )).ToList();

            if (storageKeys == null || !storageKeys.Any())
            {
                return resultList;
            }

            var assetDetailStorageChangeSets = await client.State.GetQueryStorageAtAsync(assetDetailKeys, string.Empty, token);
            var assetMetadataStorageChangeSets = await client.State.GetQueryStorageAtAsync(assetMetadataKeys, string.Empty, token);
            var assetAccountStorageChangeSets = await client.State.GetQueryStorageAtAsync(assetAccountKeys, string.Empty, token);


            if (assetDetailStorageChangeSets != null)
            {
                for (int i = 0; i < assetDetailStorageChangeSets.ElementAt(0).Changes.Count(); i++)
                {
                    var assetDetailData = assetDetailStorageChangeSets.ElementAt(0).Changes[i];
                    var assetMetadataData = assetMetadataStorageChangeSets.ElementAt(0).Changes[i];
                    var assetAccountData = assetAccountStorageChangeSets.ElementAt(0).Changes[i];

                    // If it is null, then I do not care about it.
                    if (assetDetailData[1] == null || assetMetadataData[1] == null)
                    {
                        continue;
                    }


                    string storageKeyString = storageKeys.ElementAt(i);

                    BigInteger assetId = HashModel.GetBigIntegerFromBlake2_128Concat(storageKeyString);

                    var assetDetails = new PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails();
                    assetDetails.Create(assetDetailData[1]);

                    var assetMetadata = new PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1();
                    assetMetadata.Create(assetMetadataData[1]);


                    if (assetAccountData[1] != null)
                    {
                        var assetAccount = new PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount();
                        assetAccount.Create(assetAccountData[1]);

                        resultList.Add((assetId, assetDetails, assetMetadata, assetAccount));
                    }
                    else
                    {
                        resultList.Add((assetId, assetDetails, assetMetadata, null));
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public async static Task<List<HydrationTokenData>> GetHydrationTokensBalance(SubstrateClient client, string substrateAddress, CancellationToken token)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            var tokensKeyBytes = RequestGenerator.GetStorageKeyBytesHash("Tokens", "Accounts");
            var assetRegistryKeyBytes = RequestGenerator.GetStorageKeyBytesHash("AssetRegistry", "Assets");

            byte[] prefix = tokensKeyBytes.Concat(HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
            byte[] startKey = null;

            List<string[]> storageTokensChanges = new List<string[]>();
            List<string[]> storageAssetRegistryChanges = new List<string[]>();
            List<string> storageKeys = new List<string>();

            int prefixLength = Utils.Bytes2HexString(prefix).Length;

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAsync(prefix, 1000, startKey, string.Empty, token);

                if (keysPaged == null || !keysPaged.Any())
                {
                    break;
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);
                    storageTokensChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));

                    var tar = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p =>
                    {

                        U32 tokenId = HashModel.GetU32FromTwox_64Concat(p.ToString().Substring(prefixLength));

                        var blake2Hash = HashExtension.Blake2Concat(tokenId.Encode(), 128);

                        return Utils.HexToByteArray(Utils.Bytes2HexString(assetRegistryKeyBytes) + Utils.Bytes2HexString(blake2Hash, Utils.HexStringFormat.Pure));
                    }).ToList()
                    , string.Empty, token);
                    storageAssetRegistryChanges.AddRange(new List<string[]>(tar.ElementAt(0).Changes));

                    storageKeys.AddRange(keysPaged.Select(p => p.ToString().Substring(prefixLength)).ToList());

                    startKey = Utils.HexToByteArray(tt.ElementAt(0).Changes.Last()[0]);
                }
            }

            var resultList = new List<HydrationTokenData>();

            if (storageTokensChanges != null)
            {
                for (int i = 0; i < storageTokensChanges.Count(); i++)
                {
                    var accountData = new Hydration.NetApi.Generated.Model.orml_tokens.AccountData();
                    accountData.Create(storageTokensChanges[i][1]);

                    var assetMetadata = new Hydration.NetApi.Generated.Model.pallet_asset_registry.types.AssetDetails();
                    assetMetadata.Create(storageAssetRegistryChanges[i][1]);

                    BigInteger assetId = Model.HashModel.GetBigIntegerFromTwox_64Concat(storageKeys[i]);

                    resultList.Add(new HydrationTokenData
                    {
                        AssetId = assetId,
                        AccountData = accountData,
                        AssetMetadata = assetMetadata,
                    });
                }
            }
            return resultList;
        }

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public async static Task<List<BifrostTokenData>> GetBifrostTokensBalance(SubstrateClient client, string substrateAddress, CancellationToken token)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(substrateAddress));

            var tokensKeyBytes = RequestGenerator.GetStorageKeyBytesHash("Tokens", "Accounts");
            var assetRegistryKeyBytes = RequestGenerator.GetStorageKeyBytesHash("AssetRegistry", "CurrencyMetadatas");

            byte[] prefix = tokensKeyBytes.Concat(HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
            byte[] startKey = null;

            List<string[]> storageTokensChanges = new List<string[]>();
            List<string[]> storageAssetRegistryChanges = new List<string[]>();
            List<string> storageKeys = new List<string>();

            int prefixLength = Utils.Bytes2HexString(prefix).Length;

            while (true)
            {
                var keysPaged = await client.State.GetKeysPagedAsync(prefix, 1000, startKey, string.Empty, token);

                if (keysPaged == null || !keysPaged.Any())
                {
                    break;
                }
                else
                {
                    var tt = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), string.Empty, token);
                    storageTokensChanges.AddRange(new List<string[]>(tt.ElementAt(0).Changes));

                    var tar = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => Utils.HexToByteArray(Utils.Bytes2HexString(assetRegistryKeyBytes) + p.ToString().Substring(prefixLength))).ToList(), string.Empty, token);
                    storageAssetRegistryChanges.AddRange(new List<string[]>(tar.ElementAt(0).Changes));

                    storageKeys.AddRange(keysPaged.Select(p => p.ToString().Substring(prefixLength)).ToList());

                    startKey = Utils.HexToByteArray(tt.ElementAt(0).Changes.Last()[0]);
                }
            }

            var resultList = new List<BifrostTokenData>();

            if (storageTokensChanges != null)
            {
                for (int i = 0; i < storageTokensChanges.Count(); i++)
                {
                    AccountData accountData = new AccountData();
                    accountData.Create(storageTokensChanges[i][1]);

                    var assetMetadata = new Bifrost.NetApi.Generated.Model.bifrost_asset_registry.pallet.AssetMetadata();
                    assetMetadata.Create(storageAssetRegistryChanges[i][1]);

                    BigInteger assetId = Model.HashModel.GetBigIntegerFromTwox_64Concat(storageKeys[i]);

                    resultList.Add(new BifrostTokenData
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
    public class BifrostTokenData
    {
        public BigInteger AssetId { get; set; }
        public AccountData AccountData { get; set; }
        public Bifrost.NetApi.Generated.Model.bifrost_asset_registry.pallet.AssetMetadata AssetMetadata { get; set; }
    }

    public class HydrationTokenData
    {
        public BigInteger AssetId { get; set; }
        public Hydration.NetApi.Generated.Model.orml_tokens.AccountData AccountData { get; set; }
        public Hydration.NetApi.Generated.Model.pallet_asset_registry.types.AssetDetails AssetMetadata { get; set; }
    }
}

