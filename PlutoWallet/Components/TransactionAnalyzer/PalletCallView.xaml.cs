using PlutoWallet.Constants;

namespace PlutoWallet.Components.TransactionAnalyzer;

public partial class PalletCallView : ContentView
{
    public static readonly BindableProperty PalletCallNameProperty = BindableProperty.Create(
       nameof(PalletCallName), typeof(string), typeof(PalletCallView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) =>
       {
           var control = (PalletCallView)bindable;

           control.palletCallLabel.Text = (string)newValue;
       });

    public static readonly BindableProperty EndpointProperty = BindableProperty.Create(
       nameof(Endpoint), typeof(Endpoint), typeof(PalletCallView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) =>
       {
           var control = (PalletCallView)bindable;

           var endpoint = (Endpoint)newValue;
           control.networkBubble.EndpointKey = endpoint.Key;
           control.networkBubble.Name = endpoint.Name;
           control.networkBubble.Icon = endpoint.Icon;
       });

    public PalletCallView()
    {
        InitializeComponent();
    }

    public string PalletCallName
    {
        get => (string)GetValue(PalletCallNameProperty);
        set => SetValue(PalletCallNameProperty, value);
    }

    public Endpoint Endpoint
    {
        get => (Endpoint)GetValue(EndpointProperty);
        set => SetValue(EndpointProperty, value);
    }
}