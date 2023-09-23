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
        private double heightRequest;

        public bool DoNotReload { get; set; } = false;

        [ObservableProperty]
        private string usdSum;

        [ObservableProperty]
        private bool reloadIsVisible;

        public Action<List<AssetAmountView>> ReloadBalanceViewStackLayout { get; set; }

        public UsdBalanceViewModel()
		{
            heightRequest = EXTRA_HEIGHT;
            usdSum = "Loading";
            reloadIsVisible = false;
        }

        public void UpdateBalances()
        {
            ReloadIsVisible = false;

            UsdSum = "Loading";

            var assets = new List<AssetAmountView>();

            foreach (Asset asset in Model.AssetsModel.Assets)
            {
                assets.Add(new AssetAmountView
                {
                    Amount = String.Format("{0:0.00}", asset.Amount),
                    Symbol = asset.Symbol,
                    ChainIcon = asset.ChainIcon,
                    UsdValue = String.Format("{0:0.00}", asset.UsdValue) + " USD",
                });
            }

            int count = assets.Count() < 10 ? assets.Count() : 10;

            ReloadBalanceViewStackLayout(assets);

            HeightRequest = (35 * count) + UsdBalanceViewModel.EXTRA_HEIGHT;

            var balanceDashboardViewModel = DependencyService.Get<BalanceDashboardViewModel>();

            balanceDashboardViewModel.RecalculateHeightRequest();

            UsdSum = String.Format("{0:0.00}", Model.AssetsModel.UsdSum) + " USD";

            ReloadIsVisible = true;
        }
	}
}

