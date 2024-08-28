using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Types;

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
            assets = new ObservableCollection<AssetInfo>();
            usdSum = "Loading";
            reloadIsVisible = false;
        }

        public void UpdateBalances()
        {
            var tempAssets = new ObservableCollection<AssetInfo>();

            foreach (Asset a in Model.AssetsModel.AssetsDict.Values)
            {
                if (a.Amount > 0 || a.Pallet == AssetPallet.Native)
                {
                    tempAssets.Add(new AssetInfo
                    {
                        Amount = String.Format("{0:0.00}", a.Amount),
                        Symbol = a.Symbol,
                        UsdValue = a.UsdValue > 0 ? String.Format("{0:0.00}", a.UsdValue) + " USD" : "~ USD",
                        ChainIcon = Application.Current.UserAppTheme != AppTheme.Dark ? a.ChainIcon : a.DarkChainIcon,
                    });
                }
            }

            Assets = tempAssets;

            UsdSum = String.Format("{0:0.00}", Model.AssetsModel.UsdSum) + " USD";
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

