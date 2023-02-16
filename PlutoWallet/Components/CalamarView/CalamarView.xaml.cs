namespace PlutoWallet.Components.CalamarView;

public partial class CalamarView : ContentView
{
    private string calamarWebAddress;

    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(CalamarView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (CalamarView)bindable;
            control.calamarWebView.Source = "https://f4c3cf83.calamar.pages.dev/polkadot/account/" + (string)newValue;
            control.CalamarWebAddress = "https://f4c3cf83.calamar.pages.dev/polkadot/account/" + (string)newValue;
        });

    public CalamarView()
	{
		InitializeComponent();
	}

    public string Address
    {
        get => (string)GetValue(AddressProperty);

        set => SetValue(AddressProperty, value);
    }

    public string CalamarWebAddress
    {
        get => calamarWebAddress;

        set
        {
            calamarWebAddress = value;
        }
    }

    void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        calamarWebView.Source = calamarWebAddress;
        //calamarWebView.Reload();
    }

    async void OnOpenClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Launcher.OpenAsync(calamarWebAddress);
    }
}
