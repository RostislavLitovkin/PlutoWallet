using Markdig;
using PlutoWallet.Components.Buttons;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using System.Collections;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;
using static PlutoWallet.Model.NftsStorageModel;

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
                null => "darkbackground2.png",
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

    private StorageNFT GetStorageNft()
    {
        return new StorageNFT
        {
            Name = this.NftBase.Metadata.Name,
            Description = this.NftBase.Metadata.Description,
            Image = this.NftBase.Metadata.Image,
            EndpointKey = this.Endpoint.Key,
            Attributes = [], // TODO: this.NftBase.Metadata.Attributes,
            CollectionId = this.NftBase.CollectionId.ToString(),
            ItemId = this.NftBase.Id.ToString(),
            Favourite = this.Favourite,
        };
    }

    void OnFavouriteClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Favourite = !Favourite;
        if (Favourite)
        {
            NftsStorageModel.AddFavourite(GetStorageNft());
        }
        else
        {
            NftsStorageModel.RemoveFavourite(GetStorageNft());
        }
    }

    async void OnMoreClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            var viewModel = new NftDetailViewModel();

            viewModel.Endpoint = this.Endpoint;
            viewModel.CollectionId = this.NftBase.CollectionId;
            viewModel.ItemId = this.NftBase.Id;
            viewModel.Favourite = this.Favourite;
            viewModel.OwnerAddress = this.NftBase.Owner;

            UpdateViewModel(viewModel, this.NftBase);

            if (this.Endpoint.Name == "Aleph Zero Testnet")
            {
                viewModel.AzeroIdReservedUntil = await Model.AzeroId.AzeroIdModel.GetReservedUntilStringForName(this.NftBase.Metadata.Name);
            }

            await Navigation.PushAsync(new NftDetailPage(viewModel));

            // load these details after
            viewModel.KodadotUnlockableUrl = await Model.Kodadot.UnlockablesModel.FetchKeywiseAsync(this.Endpoint, this.NftBase.CollectionId);

            CancellationToken token = CancellationToken.None;

            var collection = await Model.CollectionModel.ToCollectionWrapperAsync(await this.NftBase.GetCollectionAsync(token), CancellationToken.None);

            viewModel.CollectionBase = collection.CollectionBase;
            viewModel.CollectionFavourite = false; //NftsStorageModel.IsCollectionFavourite(collection);
            viewModel.CollectionNftImages = collection.NftImages;

            var fullCollection = await collection.CollectionBase.GetFullAsync(token);
            if (fullCollection is ICollectionStats)
            {
                viewModel.FloorPrice = ((ICollectionStats)fullCollection).FloorPrice;
                viewModel.HighestSale = ((ICollectionStats)fullCollection).HighestSale;
                viewModel.Volume = ((ICollectionStats)fullCollection).Volume;
            }

            // These can be implemented only on the full variant...
            var fullNft = await this.NftBase.GetFullAsync(token);

            UpdateViewModel(viewModel, fullNft);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private void UpdateViewModel(NftDetailViewModel viewModel, INftBase nft)
    {
        Console.WriteLine("Is for sale: " + (nft is INftBuyable));
        viewModel.Price = (nft is INftBuyable && ((INftBuyable)nft).IsForSale) ? ((INftBuyable)nft).Price ?? 0 : 0;
        viewModel.IsForSale = nft is INftBuyable && ((INftBuyable)nft).IsForSale;
        viewModel.KodaIsVisible = nft is IKodaLink;
        viewModel.UniqueIsVisible = nft is IUniqueMarketplaceLink;

        viewModel.TransferButtonState = nft is INftTransferable && ((INftTransferable)nft).IsTransferable ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
        viewModel.SellButtonState = nft is INftSellable /* && ((INftSellable)this.NftBase) */ ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;
        viewModel.ModifyButtonState = ButtonStateEnum.Disabled; // Maybe later
        viewModel.BurnButtonState = nft is INftBurnable && ((INftBurnable)nft).IsBurnable ? ButtonStateEnum.Enabled : ButtonStateEnum.Disabled;

        viewModel.NftBase = nft;
    }
}