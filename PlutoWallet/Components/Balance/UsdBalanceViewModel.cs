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
        public bool DoNotReload { get; set; } = false;

        [ObservableProperty]
        private ObservableCollection<AssetInfo> assets;

        [ObservableProperty]
        private string usdSum;

        [ObservableProperty]
        private bool reloadIsVisible;

        public UsdBalanceViewModel()
		{
            usdSum = "Loading";
            reloadIsVisible = false;
        }

        public void UpdateBalances()
        {
            ReloadIsVisible = false;

            UsdSum = "Loading";

            var tempAssets = new List<AssetInfo>();

            foreach (Asset a in Model.AssetsModel.Assets)
            {
                if (a.Amount > 0 || a.Pallet == AssetPallet.Native)
                {
                    tempAssets.Add(new AssetInfo
                    {
                        Amount = String.Format("{0:0.00}", a.Amount),
                        Symbol = a.Symbol,
                        UsdValue = String.Format("{0:0.00}", a.UsdValue) + " USD",
                        ChainIcon = a.ChainIcon
                    });
                }
            }

            Assets = new ObservableCollection<AssetInfo>(tempAssets);

            UsdSum = String.Format("{0:0.00}", Model.AssetsModel.UsdSum) + " USD";

            ReloadIsVisible = true;
        }
	}

    public class AssetInfo
    {
        public string Amount { get; set; }
        public string Symbol { get; set; }
        public string UsdValue { get; set; }
        public string ChainIcon { get; set; }
    }
}

