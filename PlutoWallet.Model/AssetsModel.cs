using System;
using System.Numerics;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using PlutoWallet.Types;
using PlutoWallet.Constants;
using Bifrost.NetApi.Generated.Model.orml_tokens;
using Substrate.NetApi.Model.Types.Primitive;
using Bifrost.NetApi.Generated.Model.bifrost_asset_registry.pallet;

namespace PlutoWallet.Model
{
    public class AssetsModel
    {
        private static bool doNotReload = false;

        public static List<Asset> Assets = new List<Asset>();

        public static double UsdSum = 0.0;

        public static List<Asset> GetAssetsWithSymbol(string symbol)
        {
            return Assets.FindAll((asset) => asset.Symbol.Equals(symbol));
        }

        public static async Task GetBalance(SubstrateClientExt[] groupClients, Endpoint[] groupEndpoints, string substrateAddress)
        {
            if (doNotReload)
            {
                return;
            }

            var tempAssets = new List<Asset>();

            double usdSumValue = 0;

            for (int i = 0; i < groupClients.Length; i++)
            {
                if (groupClients[i] is null)
                {
                    continue;
                }

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

                try
                {
                    foreach ((BigInteger, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount) asset in await GetPolkadotAssetHubAssetsAsync(client, substrateAddress, 1000, CancellationToken.None))
                    {
                        double usdRatio = 0;

                        double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value / Math.Pow(10, asset.Item3.Decimals.Value) : 0.0;

                        tempAssets.Add(new Asset
                        {
                            Amount = assetBalance,
                            Symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value),
                            ChainIcon = endpoint.Icon,
                            DarkChainIcon = endpoint.DarkIcon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Assets,
                            AssetId = asset.Item1,
                            UsdValue = assetBalance * usdRatio,
                            Decimals = asset.Item3.Decimals.Value,
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                try
                {
                    if (endpoint.Key != "bifrost")
                    {
                        foreach (HydrationTokenData tokenData in await GetHydrationTokensBalance(client, substrateAddress, CancellationToken.None))
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
                    else
                    {
                        foreach (BifrostTokenData tokenData in await GetBifrostTokensBalance(client, substrateAddress, CancellationToken.None))
                        {
                            double usdRatio = 0;

                            double assetBalance = (double)tokenData.AccountData.Free.Value / Math.Pow(10, tokenData.AssetMetadata.Decimals.Value);

                            tempAssets.Add(new Asset
                            {
                                Amount = assetBalance,
                                Symbol = Model.ToStringModel.VecU8ToString(tokenData.AssetMetadata.Symbol.Value),
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
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

            for (int i = 0; i < Assets.Count(); i++)
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
        public static async Task<List<(BigInteger, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetDetails, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetMetadataT1, PolkadotAssetHub.NetApi.Generated.Model.pallet_assets.types.AssetAccount)>> GetPolkadotAssetHubAssetsAsync(SubstrateClientExt client, string substrateAddress, uint page, CancellationToken token)
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
        public async static Task<List<TokenData>> GetTokensBalance(SubstrateClientExt client, string substrateAddress, CancellationToken token)
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

            var resultList = new List<TokenData>();

            if (storageTokensChanges != null)
            {
                for (int i = 0; i < storageTokensChanges.Count(); i++)
                {
                    Hydration.NetApi.Generated.Model.orml_tokens.AccountData accountData = new Hydration.NetApi.Generated.Model.orml_tokens.AccountData();
                    accountData.Create(storageTokensChanges[i][1]);

                    var assetMetadata = new Substrate.NetApi.Generated.Model.pallet_asset_registry.types.AssetMetadata();
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

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public async static Task<List<HydrationTokenData>> GetHydrationTokensBalance(SubstrateClientExt client, string substrateAddress, CancellationToken token)
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

                    var tar = await client.State.GetQueryStorageAtAsync(keysPaged.Select(p => {

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
        public async static Task<List<BifrostTokenData>> GetBifrostTokensBalance(SubstrateClientExt client, string substrateAddress, CancellationToken token)
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

                    AssetMetadata assetMetadata = new AssetMetadata();
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

    public class TokenData {
        public BigInteger AssetId { get; set; }
        public Hydration.NetApi.Generated.Model.orml_tokens.AccountData AccountData { get; set; }
        public Substrate.NetApi.Generated.Model.pallet_asset_registry.types.AssetMetadata AssetMetadata { get; set; }
    }

    public class BifrostTokenData
    {
        public BigInteger AssetId { get; set; }
        public AccountData AccountData { get; set; }
        public AssetMetadata AssetMetadata { get; set; }
    }

    public class HydrationTokenData
    {
        public BigInteger AssetId { get; set; }
        public Hydration.NetApi.Generated.Model.orml_tokens.AccountData AccountData { get; set; }
        public Hydration.NetApi.Generated.Model.pallet_asset_registry.types.AssetDetails AssetMetadata { get; set; }
    }
}

