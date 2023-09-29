using System;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.NetApiExt.Generated.Model.pallet_assets.types;
using System.Numerics;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.Model.AjunaExt;
using static Substrate.NetApi.Model.Meta.Storage;
using PlutoWallet.NetApiExt.Generated.Model.orml_tokens;

namespace PlutoWallet.Model
{
    public class AssetsModel
    {
        private static bool doNotReload = false;

        public static List<Asset> Assets;

        public static double UsdSum = 0.0;

        public static async Task GetBalance()
        {
            if (doNotReload)
            {
                return;
            }

            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

            usdBalanceViewModel.UsdSum = "Loading";

            var tempAssets = new List<Asset>();

            double usdSumValue = 0;

            for (int i = 0; i < Model.AjunaClientModel.GroupClients.Length; i++)
            {
                AjunaClientExt client = Model.AjunaClientModel.GroupClients[i];
                var endpoint = Model.AjunaClientModel.GroupEndpoints[i];

                if (endpoint.ChainType != Constants.ChainType.Substrate)
                {
                    /*tempAssets.Add(new Asset
                    {
                        Amount = "Unsupported",
                        //Symbol = endpoint.Unit, // I think it looks better without it
                        //ChainIcon = endpoint.Icon,
                        Endpoint = endpoint,
                        UsdValue = String.Format("{0:0.00}", 0) + " USD",
                    });*/

                    continue;
                }

                double amount = 0;

                try
                {
                    var accountInfo = await client.SystemStorage.Account(Model.KeysModel.GetSubstrateKey());

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
                        Endpoint = endpoint,
                        Pallet = AssetPallet.Native,
                        AssetId = 0,
                        UsdValue = usdValue,
                        Decimals = endpoint.Decimals,
                    });
                }

                try
                {

                    foreach ((BigInteger, AssetDetails, AssetMetadata, AssetAccount) asset in await client.AssetsStorage.GetAssetsMetadataAndAcountNextAsync(Model.KeysModel.GetSubstrateKey(), 1000, CancellationToken.None))
                    {
                        double usdRatio = 0;

                        double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value / Math.Pow(10, asset.Item3.Decimals.Value) : 0.0;

                        tempAssets.Add(new Asset
                        {
                            Amount = assetBalance,
                            Symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value.Value),
                            ChainIcon = endpoint.Icon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Assets,
                            AssetId = asset.Item1,
                            UsdValue = assetBalance * usdRatio,
                            Decimals = asset.Item3.Decimals.Value,
                        });
                    }
                    
                    foreach (TokenData tokenData in await GetTokensBalance(client))
                    {
                        double usdRatio = 0;

                        double assetBalance = (double)tokenData.AccountData.Free.Value / Math.Pow(10, tokenData.AssetMetadata.Decimals.Value);

                        tempAssets.Add(new Asset
                        {
                            Amount = assetBalance,
                            Symbol = Model.ToStringModel.VecU8ToString(tokenData.AssetMetadata.Symbol.Value.Value),
                            ChainIcon = endpoint.Icon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Tokens,
                            AssetId = tokenData.AssetId,
                            UsdValue = assetBalance * usdRatio,
                            Decimals = tokenData.AssetMetadata.Decimals.Value,
                        });
                    }
                }
                catch (Exception ex)
                {
                    var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                    messagePopup.Title = "Loading Assets Error";
                    messagePopup.Text = ex.Message;

                    messagePopup.IsVisible = true;
                }
            }

            UsdSum = usdSumValue;

            Assets = tempAssets;

            usdBalanceViewModel.UpdateBalances();
        }

        /// <summary>
        /// This is a helper function for querying Tokens balance
        /// </summary>
        /// <returns></returns>
        public async static Task<List<TokenData>> GetTokensBalance(AjunaClientExt client)
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom(Model.KeysModel.GetSubstrateKey()));

            var tokensKeyBytes = RequestGenerator.GetStorageKeyBytesHash("Tokens", "Accounts");
            var assetRegistryKeyBytes = RequestGenerator.GetStorageKeyBytesHash("AssetRegistry", "AssetMetadataMap");

            byte[] prefix = tokensKeyBytes.Concat(HashExtension.Hash(Hasher.BlakeTwo128Concat, account32.Encode())).ToArray();
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

                    PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata assetMetadata = new PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata();
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
        public PlutoWallet.NetApiExt.Generated.Model.pallet_asset_registry.types.AssetMetadata AssetMetadata { get; set; }
    }
}

