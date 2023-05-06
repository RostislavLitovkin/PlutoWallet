namespace PlutoWallet.Components.Balance;

public partial class AssetAmountView : ContentView
{
    public static readonly BindableProperty AssetProperty = BindableProperty.Create(
        nameof(Asset), typeof(Asset), typeof(AssetAmountView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetAmountView)bindable;
            var asset = (Asset)newValue;

            control.amountLabel.Text = String.Format("{0:0.00}", asset.Amount);
            control.symbolLabel.Text = asset.Symbol;
            control.chainIcon.Source = asset.ChainIcon;
            control.usdLabel.Text = String.Format("{0:0.00}", asset.UsdValue) + " USD";
        });

    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(string), typeof(AssetAmountView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetAmountView)bindable;

            control.amountLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty SymbolProperty = BindableProperty.Create(
        nameof(Symbol), typeof(string), typeof(AssetAmountView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetAmountView)bindable;

            control.symbolLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ChainIconProperty = BindableProperty.Create(
        nameof(ChainIcon), typeof(string), typeof(AssetAmountView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetAmountView)bindable;

            control.chainIcon.Source = (string)newValue;
        });

    public static readonly BindableProperty UsdValueProperty = BindableProperty.Create(
        nameof(UsdValue), typeof(string), typeof(AssetAmountView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AssetAmountView)bindable;

            control.usdLabel.Text = (string)newValue;
        });

    public AssetAmountView()
	{
		InitializeComponent();
	}

    public Asset Asset
    {
        get => (Asset)GetValue(AssetProperty);

        set => SetValue(AssetProperty, value);
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

    public string ChainIcon
    {
        get => (string)GetValue(ChainIconProperty);

        set => SetValue(ChainIconProperty, value);
    }

    public string UsdValue
    {
        get => (string)GetValue(UsdValueProperty);

        set => SetValue(UsdValueProperty, value);
    }
}
