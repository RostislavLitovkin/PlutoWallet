using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Components.WebView;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using System.Collections.ObjectModel;
using System.Numerics;
using UniqueryPlus.Collections;

namespace PlutoWallet.Components.Nft
{
    public partial class CollectionDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Name))]
        [NotifyPropertyChangedFor(nameof(Description))]
        [NotifyPropertyChangedFor(nameof(Image))]
        [NotifyPropertyChangedFor(nameof(Attributes))]
        [NotifyPropertyChangedFor(nameof(AttributesIsVisible))]
        private ICollectionBase collectionBase;
        public string Name => CollectionBase.Metadata?.Name ?? "Unknown";
        public string Description => CollectionBase.Metadata?.Description ?? "";
        public string Image => CollectionBase.Metadata?.Image ?? "";
        public ObservableCollection<UniqueryPlus.Attribute> Attributes => new ObservableCollection<UniqueryPlus.Attribute>(CollectionBase.Metadata?.Attributes ?? []);
        public bool AttributesIsVisible => CollectionBase.Metadata.Attributes is not null && CollectionBase.Metadata.Attributes.Length > 0;

        [ObservableProperty]
        private ObservableCollection<NftWrapper> nfts = new ObservableCollection<NftWrapper>();

        [RelayCommand]
        public async Task TransferAsync()
        {
            var collectionTransferViewModel = DependencyService.Get<NftTransferViewModel>();

            collectionTransferViewModel.EndpointKey = Endpoint.Key;
            collectionTransferViewModel.TransferFunction = ((ICollectionTransferable)CollectionBase).Transfer;
            collectionTransferViewModel.IsVisible = true;
            await collectionTransferViewModel.GetFeeAsync();
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOwned))]
        [NotifyPropertyChangedFor(nameof(OwnerAddressText))]
        private string ownerAddress;

        public bool IsOwned => OwnerAddress == KeysModel.GetSubstrateKey();

        public string OwnerAddressText => IsOwned switch
        {
            true => "You",
            false => OwnerAddress,
        };

        [RelayCommand]
        public async Task CopyAddressAsync() => await CopyAddress.CopyToClipboardAsync(OwnerAddress);

        [RelayCommand]
        public async Task OpenSubscanOwnerPageAsync() => await Application.Current.MainPage.Navigation.PushAsync(new WebViewPage($"https://www.subscan.io/account/{OwnerAddress}"));


        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private BigInteger collectionId = new BigInteger(0);

        [ObservableProperty]
        private bool favourite;

        [ObservableProperty]
        private string[] collectionNftImages;

        [ObservableProperty]
        private bool collectionFavourite;

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
        [NotifyPropertyChangedFor(nameof(IsTransferable))]
        private ButtonStateEnum transferButtonState = ButtonStateEnum.Disabled;

        public bool IsTransferable => TransferButtonState == ButtonStateEnum.Enabled;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsModifiable))]
        private ButtonStateEnum modifyButtonState = ButtonStateEnum.Disabled;
        public bool IsModifiable => ModifyButtonState == ButtonStateEnum.Enabled;

        public CollectionDetailViewModel()
        {

        }
    }
}
