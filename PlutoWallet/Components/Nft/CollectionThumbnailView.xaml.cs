using PlutoWallet.Constants;
using System.Numerics;
using UniqueryPlus.Collections;

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

    private object GetStorageCollection()
    {
        return new
        {
            EndpointKey = this.Endpoint.Key,
            CollectionBase = this.CollectionBase,
            Favourite = this.Favourite,
        };
    }

    void OnFavouriteClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

    }

    async void OnMoreClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

    }
}