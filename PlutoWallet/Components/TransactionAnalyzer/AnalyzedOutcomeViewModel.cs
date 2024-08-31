using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.Balance;
using PlutoWallet.Model;
using PlutoWallet.Types;
using System.Collections.ObjectModel;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);

namespace PlutoWallet.Components.TransactionAnalyzer
{
    public partial class AnalyzedOutcomeViewModel : ObservableObject, ISetToDefault
    {

        [ObservableProperty]
        private ObservableCollection<AssetInfoExpanded> assets;

        [ObservableProperty]
        private string loading;

        public AnalyzedOutcomeViewModel()
        {
            assets = new ObservableCollection<AssetInfoExpanded>();
            loading = "Loading";
        }

        public void UpdateAssetChanges(Dictionary<string, Dictionary<AssetKey, Asset>> assetChanges)
        {
            var tempAssets = new ObservableCollection<AssetInfoExpanded>();

            var walletAddress = Model.KeysModel.GetSubstrateKey();

            Console.WriteLine("Total of " + assetChanges[walletAddress].Values);
            foreach (Asset a in assetChanges[walletAddress].Values)
            {
                Console.WriteLine(a.Symbol);

                double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(a.Symbol);
                a.UsdValue = a.Amount * spotPrice;
                tempAssets.Add(new AssetInfoExpanded
                {
                    Amount = a.Amount switch
                    {
                        > 0 => "+" + String.Format("{0:0.00}", a.Amount),
                        _ => String.Format("{0:0.00}", a.Amount)
                    },
                    Symbol = a.Symbol,
                    UsdValue = a.UsdValue switch
                    {
                        > 0 => "+" + String.Format("{0:0.00}", a.UsdValue) + " USD",
                        _ => String.Format("{0:0.00}", a.UsdValue) + " USD",
                    },
                    UsdColor = a.UsdValue switch
                    {
                        > 0 => Colors.Green,
                        < 0 => Colors.Red,
                        _ => Colors.Gray,
                    },
                    ChainIcon = Application.Current.UserAppTheme != AppTheme.Dark ? a.ChainIcon : a.DarkChainIcon,
                });

            }

            Assets = tempAssets;
        }

        public void SetToDefault()
        {
            Loading = "Loading";
            Assets = new ObservableCollection<AssetInfoExpanded>();
        }
    }

    public class AssetInfoExpanded : AssetInfo
    {
        public Color UsdColor { get; set; }

    }
}
