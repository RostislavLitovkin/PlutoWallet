using PlutoWallet.Constants;

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

    public NftTitleView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);

        set => SetValue(NameProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);

        set => SetValue(EndpointProperty, value);
    }
}
