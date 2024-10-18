using Markdig;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using static PlutoWallet.Model.NftsStorageModel;
using UniqueryPlus.Nfts;
using UniqueryPlus.External;

namespace PlutoWallet.Components.Nft;

public partial class NftThumbnailView : ContentView
{
    public static readonly BindableProperty NftBaseProperty = BindableProperty.Create(
        nameof(NftBase), typeof(INftBase), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.OneWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftThumbnailView)bindable;

            var nftBase = (INftBase)newValue;

            control.nameLabelText.Text = nftBase.Metadata?.Name ?? "Unknown";
            control.descriptionLabel.Text = Markdown.ToHtml(nftBase.Metadata?.Description ?? "No description");
            control.image.Source = nftBase.Metadata?.Image; // Add default image if null
             // TODO: nftBase.Metadata?.Attributes ?? [];
        });

    public static readonly BindableProperty FavouriteProperty = BindableProperty.Create(
        nameof(Favourite), typeof(bool), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftThumbnailView)bindable;

            control.filledFavouriteIcon.IsVisible = (bool)newValue;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftThumbnailView)bindable;

            control.networkBubble.Name = ((Endpoint)newValue).Name;
            control.networkBubble.EndpointKey = ((Endpoint)newValue).Key;
        });

    public NftThumbnailView()
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
        var viewModel = new NftDetailViewModel();

        viewModel.NftBase = this.NftBase;

        viewModel.Name = this.NftBase.Metadata.Name;
        viewModel.Description = this.NftBase.Metadata.Description;
        viewModel.Image = this.NftBase.Metadata.Image;
        viewModel.Endpoint = this.Endpoint;
        viewModel.Attributes = []; // TODO: this.NftBase.Metadata.Attributes;
        viewModel.CollectionId = this.NftBase.CollectionId;
        viewModel.ItemId = this.NftBase.Id;
        viewModel.Favourite = this.Favourite;
        viewModel.OwnerAddress = this.NftBase.Owner;

        viewModel.KodaIsVisible = this.NftBase is IKodaLink;
        viewModel.UniqueIsVisible = this.NftBase is IUniqueMarketplaceLink;

        viewModel.IsTransferable = this.NftBase is INftTransferable && ((INftTransferable)this.NftBase).IsTransferable;
        viewModel.IsSellable = this.NftBase is INftSellable; //&& ((INftSellable)this.NftBase);
        viewModel.IsModifiable = false; // Maybe later
        viewModel.IsBurnable = this.NftBase is INftBurnable && ((INftBurnable)this.NftBase).IsBurnable;


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
    }
}
