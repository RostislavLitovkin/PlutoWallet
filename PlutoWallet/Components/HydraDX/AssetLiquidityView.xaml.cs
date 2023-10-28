namespace PlutoWallet.Components.HydraDX;

public partial class AssetLiquidityView : ContentView
{
    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(string), typeof(AssetLiquidityView ),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetLiquidityView )bindable;

            control.amountLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty SymbolProperty = BindableProperty.Create(
        nameof(Symbol), typeof(string), typeof(AssetLiquidityView ),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetLiquidityView )bindable;

            control.symbolLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty UsdValueProperty = BindableProperty.Create(
        nameof(UsdValue), typeof(string), typeof(AssetLiquidityView ),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetLiquidityView )bindable;

            control.usdLabel.Text = (string)newValue;
        });

    public AssetLiquidityView()
	{
		InitializeComponent();
	}

    public string Amount
    {
        get => (string)GetValue(AmountProperty);

        set => SetValue(AmountProperty, value);
    }

    public string Symbol
    {
        get => (string)GetValue(SymbolProperty);

        set => SetValue(SymbolProperty, value);
    }

    public string UsdValue
    {
        get => (string)GetValue(UsdValueProperty);

        set => SetValue(UsdValueProperty, value);
    }
}
