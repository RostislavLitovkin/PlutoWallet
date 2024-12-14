using Nethereum.Util;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.SQLite;
using System.Collections.ObjectModel;
using UniqueryPlus.Collections;
using UniqueryPlus.External;

namespace PlutoWallet.Components.Nft;

public partial class CollectionThumbnailView : ContentView
{
    public static readonly BindableProperty CollectionBaseProperty = BindableProperty.Create(
        nameof(CollectionBase), typeof(ICollectionBase), typeof(CollectionThumbnailView),
        defaultBindingMode: BindingMode.OneWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (CollectionThumbnailView)bindable;

            var collectionBase = (ICollectionBase)newValue;

            control.nameLabelText.Text = collectionBase.Metadata?.Name ?? "Unknown";
            control.image.Source = collectionBase.Metadata?.Image; // Add default image if null
            control.nftCountLabel.Text = collectionBase.NftCount > 999 ? "999+ items" : collectionBase.NftCount > 1 ? $"{collectionBase.NftCount} items" : "1 item";
            control.nftIdView.Id = collectionBase.CollectionId;
        }
        );

    public static readonly BindableProperty FavouriteProperty = BindableProperty.Create(
        nameof(Favourite), typeof(bool), typeof(CollectionThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (CollectionThumbnailView)bindable;

            control.filledFavouriteIcon.IsVisible = (bool)newValue;
        });

    public static readonly BindableProperty NftImagesProperty = BindableProperty.Create(
        nameof(NftImages), typeof(string[]), typeof(CollectionThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CollectionThumbnailView)bindable;

            var images = (string[])newValue;

            switch (images.Length)
            {
                case 0:
                    control.nft1Border.IsVisible = false;
                    control.nft2Border.IsVisible = false;
                    control.nft3Border.IsVisible = false;
                    control.nftCountBorder.IsVisible = false;
                    control.emptyBorder.IsVisible = true;
                    break;
                case 1:
                    control.nft1Image.Source = images[0];
                    control.nft2Border.IsVisible = false;
                    control.nft3Border.IsVisible = false;
                    break;
                case 2:
                    control.nft1Image.Source = images[0];
                    control.nft2Image.Source = images[1];
                    control.nft3Border.IsVisible = false;
                    break;
                default:
                    control.nft1Image.Source = images[0];
                    control.nft2Image.Source = images[1];
                    control.nft3Image.Source = images[2];
                    break;
            }
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(CollectionThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CollectionThumbnailView)bindable;

            if (newValue is null)
            {
                return;
            }

            control.networkBubble.Name = ((Endpoint)newValue).Name;
            control.networkBubble.EndpointKey = ((Endpoint)newValue).Key;
        });


    public CollectionThumbnailView()
    {
        InitializeComponent();
    }

    public ICollectionBase CollectionBase
    {
        get => (ICollectionBase)GetValue(CollectionBaseProperty);
        set => SetValue(CollectionBaseProperty, value);
    }

    public bool Favourite
    {
        get => (bool)GetValue(FavouriteProperty);

        set => SetValue(FavouriteProperty, value);
    }


    public string[] NftImages
    {
        get => (string[])GetValue(NftImagesProperty);

        set => SetValue(NftImagesProperty, value);
    }
    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    void OnFavouriteClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Favourite = !Favourite;
        Task save = CollectionDatabase.SaveItemAsync(new CollectionWrapper
        {
            NftImages = NftImages,
            Endpoint = Endpoint,
            CollectionBase = CollectionBase,
            Favourite = Favourite
        });
    }

    async void OnMoreClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        CancellationToken token = CancellationToken.None;

        try
        {
            var viewModel = new CollectionDetailViewModel();

            viewModel.Endpoint = this.Endpoint;
            viewModel.CollectionId = this.CollectionBase.CollectionId;
            viewModel.Favourite = this.Favourite;
            viewModel.OwnerAddress = this.CollectionBase.Owner;

            await UpdateViewModelAsync(viewModel, this.CollectionBase, token);

            await Navigation.PushAsync(new CollectionDetailPage(viewModel));

            var fullCollection = await this.CollectionBase.GetFullAsync(token);

            await UpdateViewModelAsync(viewModel, fullCollection, token);

            viewModel.Nfts = new ObservableCollection<NftWrapper>((await fullCollection.GetNftsAsync(25, null, token)).Select(Model.NftModel.ToNftWrapper));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
    public static async Task UpdateViewModelAsync(CollectionDetailViewModel viewModel, ICollectionBase collection, CancellationToken token)
    {
        if (collection is ICollectionStats)
        {
            viewModel.FloorPrice = ((ICollectionStats)collection).FloorPrice;
            viewModel.HighestSale = ((ICollectionStats)collection).HighestSale;
            viewModel.Volume = ((ICollectionStats)collection).Volume;
        }

        viewModel.KodaIsVisible = collection is IKodaLink;
        viewModel.UniqueIsVisible = collection is IUniqueMarketplaceLink;

        viewModel.TransferButtonState = collection is ICollectionTransferable && ((ICollectionTransferable)collection).IsTransferable ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
        viewModel.ModifyButtonState = ButtonStateEnum.Disabled; // Maybe later

        viewModel.CollectionBase = collection;

        if (collection is ICollectionEVMClaimable)
        {
            var eventInfo = await ((ICollectionEVMClaimable)collection).GetEventInfoAsync(token).ConfigureAwait(false);

            if (eventInfo is not null)
            {
                viewModel.EventStartTimestamp = (long)eventInfo.StartTimestamp;
                viewModel.EventEndTimestamp = (long)eventInfo.EndTimestamp;

                var timestampNow = DateTime.Now.ToUnixTimestamp();

                var canBeClaimed = (eventInfo.StartTimestamp <= timestampNow && timestampNow < eventInfo.EndTimestamp);
                Console.WriteLine("Can be claimed" + canBeClaimed);
                viewModel.ClaimButtonState = canBeClaimed ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
            }
            else
            {
                Console.WriteLine("EVM event info was null");
            }
        }
    }
}