using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.WebView;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft
{
    public partial class NftDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        [NotifyPropertyChangedFor(nameof(Description))]
        [NotifyPropertyChangedFor(nameof(Image))]
        [NotifyPropertyChangedFor(nameof(Attributes))]
        [NotifyPropertyChangedFor(nameof(AttributesIsVisible))]
        private INftBase nftBase;

        public string Name => NftBase.Metadata.Name;

        public string Description => NftBase.Metadata.Description;

        public string Image => NftBase.Metadata.Image;

        public UniqueryPlus.Attribute[] Attributes => NftBase.Metadata.Attributes;
        public bool AttributesIsVisible => Attributes.Length > 0;


        [RelayCommand]
        public async Task TransferAsync()
        {
            var nftTransferViewModel = DependencyService.Get<NftTransferViewModel>();

            nftTransferViewModel.EndpointKey = Endpoint.Key;
            nftTransferViewModel.TransferFunction = ((INftTransferable)NftBase).Transfer;
            nftTransferViewModel.IsVisible = true;
            await nftTransferViewModel.GetFeeAsync();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOwned))]
        [NotifyPropertyChangedFor(nameof(OwnerAddressText))]
        [NotifyPropertyChangedFor(nameof(BuyViewIsVisible))]
        private string ownerAddress;

        public bool IsOwned => OwnerAddress == KeysModel.GetSubstrateKey();

        public string OwnerAddressText => IsOwned switch
        {
            true => "You",
            false => OwnerAddress.Length switch
            {
                > 12 => OwnerAddress.Substring(0, 12) + "..",
                _ => OwnerAddress,
            }
        };

        [RelayCommand]
        public async Task CopyAddressAsync() => await CopyAddress.CopyToClipboardAsync(OwnerAddress);

        [RelayCommand]
        public async Task OpenSubscanOwnerPageAsync() => await Application.Current.MainPage.Navigation.PushAsync(new WebViewPage($"https://www.subscan.io/account/{OwnerAddress}"));
        

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PriceText))]
        private Endpoint endpoint;

        [ObservableProperty]
        private BigInteger collectionId = new BigInteger(0);

        [ObservableProperty]
        private BigInteger itemId = new BigInteger(0);

        [ObservableProperty]
        private bool favourite;

        [ObservableProperty]
        private Option<string> kodadotUnlockableUrl;

        [ObservableProperty]
        private string azeroIdReservedUntil;

        [ObservableProperty]
        private string[] collectionNftImages;

        [ObservableProperty]
        private bool collectionFavourite;

        [ObservableProperty]
        private ICollectionBase collectionBase;

        [ObservableProperty]
        private bool uniqueIsVisible;

        [ObservableProperty]
        private bool kodaIsVisible;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FloorPriceText))]
        [NotifyPropertyChangedFor(nameof(TradingStatsIsVisible))]
        private BigInteger floorPrice;

        public string FloorPriceText => String.Format("{0:0.00} {1}", (double)FloorPrice / double.Pow(10, Endpoint.Decimals), Endpoint.Unit);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HighestSaleText))]
        [NotifyPropertyChangedFor(nameof(TradingStatsIsVisible))]
        private BigInteger highestSale;

        public string HighestSaleText => String.Format("{0:0.00} {1}", (double)HighestSale / double.Pow(10, Endpoint.Decimals), Endpoint.Unit);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(VolumeText))]
        [NotifyPropertyChangedFor(nameof(TradingStatsIsVisible))]
        private BigInteger volume;

        public string VolumeText => String.Format("{0:0.00} {1}", (double)Volume / double.Pow(10, Endpoint.Decimals), Endpoint.Unit);
        public bool TradingStatsIsVisible => HighestSale > 0 || Volume > 0 || FloorPrice > 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(BuyViewIsVisible))]
        [NotifyPropertyChangedFor(nameof(SoldForViewIsVisible))]
        private bool isForSale = false;

        public bool BuyViewIsVisible => IsForSale && !IsOwned;
        public bool SoldForViewIsVisible => IsForSale && IsOwned;


        [RelayCommand]
        public async Task BuyAsync()
        {
            var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(Endpoint.Key);

            var client = clientExt.SubstrateClient;

            try
            {
                Method buy = ((INftBuyable)NftBase).Buy();

                var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

                await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, buy, false);
            }
            catch (Exception ex)
            {
                //errorLabel.Text = ex.Message;
                Console.WriteLine(ex);
            }
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PriceText))]
        private BigInteger price;

        public string PriceText => String.Format("{0:0.00} {1}", (double)Price / double.Pow(10, Endpoint.Decimals), Endpoint.Unit);

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsTransferable))]
        private ButtonStateEnum transferButtonState = ButtonStateEnum.Disabled;

        public bool IsTransferable => TransferButtonState == ButtonStateEnum.Enabled;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSellable))]
        private ButtonStateEnum sellButtonState = ButtonStateEnum.Disabled;

        public bool IsSellable => SellButtonState == ButtonStateEnum.Enabled;

        [RelayCommand]
        public async Task SellAsync()
        {
            var nftSellViewModel = DependencyService.Get<NftSellViewModel>();

            nftSellViewModel.Endpoint = Endpoint;
            nftSellViewModel.SellFunction = ((INftSellable)NftBase).Sell;
            nftSellViewModel.IsVisible = true;
            await nftSellViewModel.GetFeeAsync(Endpoint.Key, NftBase);
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsModifiable))]
        private ButtonStateEnum modifyButtonState = ButtonStateEnum.Disabled;
        public bool IsModifiable => ModifyButtonState == ButtonStateEnum.Enabled;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsBurnable))]
        private ButtonStateEnum burnButtonState = ButtonStateEnum.Disabled;
        public bool IsBurnable => BurnButtonState == ButtonStateEnum.Enabled;
        public NftDetailViewModel()
        {
        }
    }
}
