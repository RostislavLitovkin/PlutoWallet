using PlutoWallet.Constants;

namespace PlutoWallet.Components.TransactionAnalyzer;

public partial class DAppNetworkView : ContentView
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(DAppNetworkView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (DAppNetworkView)bindable;

            control.icon.Source = (string)newValue;
        });

    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(DAppNetworkView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (DAppNetworkView)bindable;

            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
        nameof(Endpoint), typeof(Endpoint), typeof(DAppNetworkView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (DAppNetworkView)bindable;

            var endpoint = (Endpoint)newValue;
            control.networkBubble.EndpointKey = endpoint.Key;
            control.networkBubble.Name = endpoint.Name;
            control.networkBubble.Icon = endpoint.Icon;

        });
    public DAppNetworkView()
    {
        InitializeComponent();
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
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