using PlutoWallet.Components.Buttons;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.SQLite;
using System.Collections.ObjectModel;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft;

public partial class NftLittleThumbnailView : ContentView
{
    public static readonly BindableProperty NftBaseProperty = BindableProperty.Create(
        nameof(NftBase), typeof(INftBase), typeof(NftLittleThumbnailView),
        defaultBindingMode: BindingMode.OneWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftLittleThumbnailView)bindable;

            var nftBase = (INftBase)newValue;

            control.nftIdView.Id = nftBase.Id;

            control.nameLabelText.Text = nftBase.Metadata?.Name ?? "Unknown";
            control.image.Source = nftBase.Metadata?.Image[0..4] switch
            {
                // Default image
                null => "noimage.png",
                "http" => new UriImageSource
                {
                    Uri = new Uri(nftBase.Metadata.Image),
                    CacheValidity = new TimeSpan(1, 0, 0),
                },
                _ => nftBase.Metadata.Image
            };

            // TODO: nftBase.Metadata?.Attributes ?? [];
        });

    public static readonly BindableProperty FavouriteProperty = BindableProperty.Create(
        nameof(Favourite), typeof(bool), typeof(NftLittleThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftLittleThumbnailView)bindable;

            control.filledFavouriteIcon.IsVisible = (bool)newValue;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(NftLittleThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftLittleThumbnailView)bindable;

            control.networkBubble.Name = ((Endpoint)newValue).Name;
            control.networkBubble.EndpointKey = ((Endpoint)newValue).Key;
        });

    public static readonly BindableProperty PriceProperty = BindableProperty.Create(
        nameof(Price), typeof(AssetInfoExpanded), typeof(NftLittleThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftLittleThumbnailView)bindable;

            if (newValue is null)
            {
                return;
            }

            Grid.SetColumnSpan(control.nftInfoLayout, 1);

            var price = (AssetInfoExpanded)newValue;

            control.usdLabel.Text = price.UsdValue;
            control.usdLabel.TextColor = price.UsdColor;
        });

    public static readonly BindableProperty OperationProperty = BindableProperty.Create(
        nameof(Operation), typeof(NftOperation), typeof(NftLittleThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftLittleThumbnailView)bindable;

            control.operationLabel.Text = (NftOperation)newValue switch
            {
                NftOperation.Received => "Received",
                NftOperation.Sent => "Sent",
                _ => "",
            };
        });
    public NftLittleThumbnailView()
    {
        InitializeComponent();
    }
    public INftBase NftBase
    {
        get => (INftBase)GetValue(NftBaseProperty);
        set => SetValue(NftBaseProperty, value);
    }

    public bool Favourite
    {
        get => (bool)GetValue(FavouriteProperty);

        set => SetValue(FavouriteProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    public AssetInfoExpanded Price
    {
        get => (AssetInfoExpanded)GetValue(PriceProperty);

        set => SetValue(PriceProperty, value);
    }

    public NftOperation Operation
    {
        get => (NftOperation)GetValue(OperationProperty);

        set => SetValue(OperationProperty, value);
    }
    void OnFavouriteClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Favourite = !Favourite;
        Task save = NftDatabase.SaveItemAsync(new NftWrapper
        {
            Endpoint = Endpoint,
            NftBase = NftBase,
            Favourite = Favourite
        });
    }

    async void OnMoreClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        CancellationToken token = CancellationToken.None;
        try
        {
            var viewModel = new NftDetailViewModel();

            viewModel.Endpoint = this.Endpoint;
            viewModel.CollectionId = this.NftBase.CollectionId;
            viewModel.ItemId = this.NftBase.Id;
            viewModel.Favourite = this.Favourite;
            viewModel.OwnerAddress = this.NftBase.Owner;

            var savedCollection = await CollectionDatabase.GetCollectionAsync($"{this.NftBase.Type}-{this.NftBase.CollectionId}");

            if (savedCollection is not null)
            {
                viewModel.CollectionFavourite = savedCollection.Favourite;
                viewModel.CollectionBase = savedCollection.CollectionBase;
                viewModel.CollectionNftImages = savedCollection.NftImages;
            }

            await UpdateViewModelAsync(viewModel, this.NftBase, token);

            if (this.Endpoint.Key == EndpointEnum.AzeroTestnet)
            {
                viewModel.AzeroIdReservedUntil = await Model.AzeroId.AzeroIdModel.GetReservedUntilStringForName(this.NftBase.Metadata.Name).ConfigureAwait(false);
            }

            await Navigation.PushAsync(new NftDetailPage(viewModel));

            // load these details after
            viewModel.KodadotUnlockableUrl = await Model.Kodadot.UnlockablesModel.FetchKeywiseAsync(this.Endpoint, this.NftBase.CollectionId).ConfigureAwait(false);

            ICollectionBase fullCollection;
            if (savedCollection is null)
            {
                var collection = await Model.CollectionModel.ToCollectionWrapperAsync(await this.NftBase.GetCollectionAsync(token).ConfigureAwait(false), CancellationToken.None).ConfigureAwait(false);

                viewModel.CollectionBase = collection.CollectionBase;
                viewModel.CollectionNftImages = collection.NftImages;

                fullCollection = await collection.CollectionBase.GetFullAsync(token).ConfigureAwait(false);

            }
            else
            {
                fullCollection = await savedCollection.CollectionBase.GetFullAsync(token).ConfigureAwait(false);
            }

            if (fullCollection is ICollectionStats)
            {
                viewModel.FloorPrice = ((ICollectionStats)fullCollection).FloorPrice;
                viewModel.HighestSale = ((ICollectionStats)fullCollection).HighestSale;
                viewModel.Volume = ((ICollectionStats)fullCollection).Volume;
            }

            // These can be implemented only on the full variant...
            var fullNft = await this.NftBase.GetFullAsync(token);

            await UpdateViewModelAsync(viewModel, fullNft, token);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task UpdateViewModelAsync(NftDetailViewModel viewModel, INftBase nft, CancellationToken token)
    {
        viewModel.NftBase = nft;

        viewModel.Price = (nft is INftBuyable && ((INftBuyable)nft).IsForSale) ? ((INftBuyable)nft).Price ?? 0 : 0;
        viewModel.IsForSale = nft is INftBuyable && ((INftBuyable)nft).IsForSale;
        viewModel.KodaIsVisible = nft is IKodaLink;
        viewModel.UniqueIsVisible = nft is IUniqueMarketplaceLink;

        viewModel.TransferButtonState = nft is INftTransferable && ((INftTransferable)nft).IsTransferable ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
        viewModel.SellButtonState = nft is INftSellable /* && ((INftSellable)this.NftBase) */ ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
        viewModel.ModifyButtonState = ButtonStateEnum.Disabled; // Maybe later
        viewModel.BurnButtonState = nft is INftBurnable && ((INftBurnable)nft).IsBurnable ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;

        if (nft is INftNestable && !viewModel.IsNestable)
        {
            viewModel.IsNestable = true;
            viewModel.NestedNfts = new ObservableCollection<NftWrapper>((await ((INftNestable)nft).GetNestedNftsAsync(token).ConfigureAwait(false)).Select(nestedNftWrapper => Model.NftModel.ToNftWrapper(nestedNftWrapper.NftBase)));
        }
    }
}