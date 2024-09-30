namespace PlutoWallet.Components.WebView;

public partial class AdvancedWebView : ContentView
{
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(AdvancedWebView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AdvancedWebView)bindable;
            control.webView.Source = (string)newValue;
        });

    public AdvancedWebView()
	{
		InitializeComponent();
	}

    public string Address
    {
        get => (string)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }
}
