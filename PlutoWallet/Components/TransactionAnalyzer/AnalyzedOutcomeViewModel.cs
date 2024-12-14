using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.Balance;
using PlutoWallet.Model;
using PlutoWallet.Types;
using System.Collections.ObjectModel;
using AssetKey = (PlutoWallet.Constants.EndpointEnum, PlutoWallet.Types.AssetPallet, System.Numerics.BigInteger);
using NftKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger, System.Numerics.BigInteger);

namespace PlutoWallet.Components.TransactionAnalyzer
{
    public partial class AnalyzedOutcomeViewModel : ObservableObject, ISetToDefault
    {

        [ObservableProperty]
        private ObservableCollection<AssetInfoExpanded> assets = new ObservableCollection<AssetInfoExpanded>();

        [ObservableProperty]
        private ObservableCollection<NftAssetWrapperExpanded> nfts = new ObservableCollection<NftAssetWrapperExpanded>();

        [ObservableProperty]
        private string loading = "Loading";

        public void UpdateAssetChanges(Dictionary<string, Dictionary<AssetKey, Asset>> assetChanges)
        {
            var tempAssets = new ObservableCollection<AssetInfoExpanded>();

            var walletAddress = Model.KeysModel.GetSubstrateKey();

            if (!assetChanges.ContainsKey(walletAddress))
            {
                return;
            }

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

        public void UpdateNftChanges(Dictionary<string, Dictionary<NftKey, NftAssetWrapper>> nftChanges)
        {
            var tempNfts = new ObservableCollection<NftAssetWrapperExpanded>();

            var walletAddress = Model.KeysModel.GetSubstrateKey();

            if (!nftChanges.ContainsKey(walletAddress))
            {
                return;
            }

            foreach (var nft in nftChanges[walletAddress].Values)
            {
                double spotPrice = Model.HydraDX.Sdk.GetSpotPrice(nft.AssetPrice.Symbol);
                nft.AssetPrice.UsdValue = nft.AssetPrice.Amount * spotPrice;
                tempNfts.Add(new NftAssetWrapperExpanded
                {
                    NftBase = nft.NftBase,
                    Endpoint = nft.Endpoint,
                    Favourite = nft.Favourite,
                    Price = new AssetInfoExpanded
                    {

                        Amount = nft.AssetPrice.Amount switch
                        {
                            > 0 => "+" + String.Format("{0:0.00}", nft.AssetPrice.Amount),
                            _ => String.Format("{0:0.00}", nft.AssetPrice.Amount)
                        },
                        Symbol = nft.AssetPrice.Symbol,
                        UsdValue = nft.AssetPrice.UsdValue switch
                        {
                            > 0 => "+" + String.Format("{0:0.00}", nft.AssetPrice.UsdValue) + " USD",
                            _ => String.Format("{0:0.00}", nft.AssetPrice.UsdValue) + " USD",
                        },
                        UsdColor = nft.Operation switch
                        {
                            NftOperation.Received => Colors.Green,
                            NftOperation.Sent => Colors.Red,
                            _ => Colors.Gray,
                        },
                        ChainIcon = Application.Current.UserAppTheme != AppTheme.Dark ? nft.AssetPrice.ChainIcon : nft.AssetPrice.DarkChainIcon,
                    },
                    Operation = nft.Operation,
                });

            }

            Nfts = tempNfts;
        }

        public void SetToDefault()
        {
            Loading = "Loading";
            Assets = new ObservableCollection<AssetInfoExpanded>();
            Nfts = new ObservableCollection<NftAssetWrapperExpanded>();
        }
    }

    public class AssetInfoExpanded : AssetInfo
    {
        public Color UsdColor { get; set; }
    }
    public class NftAssetWrapperExpanded : NftWrapper
    {
        public NftOperation Operation { get; set; }
        public AssetInfoExpanded Price { get; set; }
    }
}
