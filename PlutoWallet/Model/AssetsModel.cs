using System;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.NetApiExt.Generated.Model.pallet_assets.types;
using System.Numerics;

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
                var client = Model.AjunaClientModel.GroupClients[i];
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
                double usdValue = amount;

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
                });

                try
                {

                    foreach ((BigInteger, AssetDetails, AssetMetadata, AssetAccount) asset in await client.AssetsStorage.GetAssetsMetadataAndAcountNextAsync(Model.KeysModel.GetSubstrateKey(), 1000, CancellationToken.None))
                    {
                        double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value : 0;
                        tempAssets.Add(new Asset
                        {
                            Amount = assetBalance,
                            Symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value.Value),
                            ChainIcon = endpoint.Icon,
                            Endpoint = endpoint,
                            Pallet = AssetPallet.Assets,
                            AssetId = asset.Item1,
                            UsdValue = assetBalance,
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
    }
}

