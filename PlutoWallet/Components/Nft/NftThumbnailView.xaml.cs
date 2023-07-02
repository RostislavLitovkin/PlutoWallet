using Markdig;
using PlutoWallet.Constants;
using PlutoWallet.Model;

namespace PlutoWallet.Components.Nft;

public partial class NftThumbnailView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftThumbnailView)bindable;

            control.nameLabelText.Text = (string)newValue;
        });

    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(
        nameof(Description), typeof(string), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftThumbnailView)bindable;

            control.descriptionLabel.Text = Markdown.ToHtml((string)newValue);
        });

    public static readonly BindableProperty ImageProperty = BindableProperty.Create(
        nameof(Image), typeof(string), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftThumbnailView)bindable;

            control.image.Source = (string)newValue;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(NftThumbnailView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftThumbnailView)bindable;

            control.networkBubble.Name = ((Endpoint)newValue).Name;
            control.networkBubble.Icon = ((Endpoint)newValue).Icon;
        });


    public NftThumbnailView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);

        set => SetValue(NameProperty, value);
    }

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);

        set => SetValue(DescriptionProperty, value);
    }

    public string Image
    {
        get => (string)GetValue(ImageProperty);

        set => SetValue(ImageProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    public NFT Nft
    {
        get;
        set;
    }

    void OnFavouriteClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
    }
}
