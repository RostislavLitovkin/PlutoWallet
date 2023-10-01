namespace PlutoWallet.Components.HydraDX;

public partial class DCAOrderView : ContentView
{
    public static readonly BindableProperty AmountProperty = BindableProperty.Create(
        nameof(Amount), typeof(string), typeof(DCAOrderView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (DCAOrderView)bindable;

            control.amountLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty FromSymbolProperty = BindableProperty.Create(
        nameof(FromSymbol), typeof(string), typeof(DCAOrderView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (DCAOrderView)bindable;

            control.fromSymbolLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ToSymbolProperty = BindableProperty.Create(
        nameof(ToSymbol), typeof(string), typeof(DCAOrderView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (DCAOrderView)bindable;

            control.toSymbolLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty TimeProperty = BindableProperty.Create(
        nameof(Time), typeof(string), typeof(DCAOrderView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (DCAOrderView)bindable;

            control.timeLabel.Text = (string)newValue;
        });

    public DCAOrderView()
	{
		InitializeComponent();
	}

    public string Amount
    {
        get => (string)GetValue(AmountProperty);

        set => SetValue(AmountProperty, value);
    }

    public string FromSymbol
    {
        get => (string)GetValue(FromSymbolProperty);

        set => SetValue(FromSymbolProperty, value);
    }

    public string ToSymbol
    {
        get => (string)GetValue(ToSymbolProperty);

        set => SetValue(ToSymbolProperty, value);
    }

    public string Time
    {
        get => (string)GetValue(TimeProperty);

        set => SetValue(TimeProperty, value);
    }
}
