using PlutoWallet.Constants;
using PlutoWallet.Model;

namespace PlutoWallet.Components.Nft;

public partial class NftTitleView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(NftTitleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftTitleView)bindable;

            control.nameText.Text = (string)newValue;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(NftTitleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftTitleView)bindable;

            control.networkBubble.Name = ((Endpoint)newValue).Name;
            control.networkBubble.Icon = ((Endpoint)newValue).Icon;
        });

    public static readonly BindableProperty KodadotUnlockableUrlProperty = BindableProperty.Create(
        nameof(KodadotUnlockableUrl), typeof(Option<string>), typeof(NftTitleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NftTitleView)bindable;
            if(((Option<string>)newValue).IsSome(out var url))
            {
                control.kodadotUnlockableButton.IsVisible = true;
            }
        });

    public NftTitleView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);

        set => SetValue(NameProperty, value);
    }

    public Option<string> KodadotUnlockableUrl
    {
        get => (Option<string>)GetValue(KodadotUnlockableUrlProperty);

        set => SetValue(KodadotUnlockableUrlProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }

    async void ClaimPhysicalDropClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if (KodadotUnlockableUrl.IsSome(out var url))
        {
            await Navigation.PushAsync(new WebView.WebViewPage(url));
        }
    }
}
