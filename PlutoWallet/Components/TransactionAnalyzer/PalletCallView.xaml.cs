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

    public PalletCallView()
    {
        InitializeComponent();
    }

    public string PalletCallName
    {
        get => (string)GetValue(PalletCallNameProperty);
        set => SetValue(PalletCallNameProperty, value);
    }
}