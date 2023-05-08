using System;
using System.Collections.ObjectModel;
using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.MessagePopup;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.NetApiExt.Generated.Model.pallet_assets.types;
using Newtonsoft.Json.Linq;

namespace PlutoWallet.Components.Balance
{
	public partial class UsdBalanceViewModel : ObservableObject
	{
        private const int EXTRA_HEIGHT = 65;

        [ObservableProperty]
        private ObservableCollection<Asset> assets = new ObservableCollection<Asset>();

        [ObservableProperty]
        private double heightRequest;

        [ObservableProperty]
        private string usdSum;

        [ObservableProperty]
        private bool reloadIsVisible;

        public UsdBalanceViewModel()
		{
            heightRequest = EXTRA_HEIGHT;
            usdSum = "Loading";
            reloadIsVisible = false;
        }

        public async Task GetBalancesAsync()
        {
            ReloadIsVisible = false;

            UsdSum = "Loading";

            double usdSumValue = 0;
            var assetsCollection = new ObservableCollection<Asset>();

            for (int i = 0; i < Model.AjunaClientModel.GroupClients.Length; i++)
            {
                var client = Model.AjunaClientModel.GroupClients[i];
                var endpoint = Model.AjunaClientModel.GroupEndpoints[i];

                if (endpoint.ChainType != Constants.ChainType.Substrate)
                {
                    assetsCollection.Add(new Asset
                    {
                        Amount = "Unsupported",
                        //Symbol = endpoint.Unit, // I think it looks better without it
                        //ChainIcon = endpoint.Icon,
                        UsdValue = String.Format("{0:0.00}", 0) + " USD",
                    });

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

                assetsCollection.Add(new Asset
                {
                    Amount = String.Format("{0:0.00}", amount),
                    Symbol = endpoint.Unit,
                    ChainIcon = endpoint.Icon,
                    UsdValue = String.Format("{0:0.00}", usdValue) + " USD",
                });

                try
                {
                    var assets = await client.AssetsStorage.GetAssetsMetadataAndAcountNextAsync(Model.KeysModel.GetSubstrateKey(), 1000, CancellationToken.None);

                    foreach ((string, AssetDetails, AssetMetadata, AssetAccount) asset in assets)
                    {

                        double assetBalance = asset.Item4 != null ? (double)asset.Item4.Balance.Value : 0;
                        assetsCollection.Add(new Asset
                        {
                            Amount = String.Format("{0:0.00}", assetBalance),
                            Symbol = Model.ToStringModel.VecU8ToString(asset.Item3.Symbol.Value.Value),
                            ChainIcon = endpoint.Icon,
                            UsdValue = String.Format("{0:0.00}", assetBalance) + " USD",
                        });
                    }
                }
                catch (Exception ex)
                {
                    var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                    messagePopup.Title = "AssetsError";
                    messagePopup.Text = ex.Message;

                    messagePopup.IsVisible = true;

                }
            }

            Assets = assetsCollection;

            int count = assetsCollection.Count() < 10 ? assetsCollection.Count() : 10;

            HeightRequest = (35 * count) + EXTRA_HEIGHT;

            var balanceDashboardViewModel = DependencyService.Get<BalanceDashboardViewModel>();

            balanceDashboardViewModel.RecalculateHeightRequest();

            UsdSum = String.Format("{0:0.00}", usdSumValue) + " USD";

            ReloadIsVisible = true;
        }
	}
}

