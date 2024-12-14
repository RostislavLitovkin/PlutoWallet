namespace PlutoWallet.Components.Date;

public partial class DateView : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
    nameof(Title), typeof(string), typeof(DateView),
    defaultBindingMode: BindingMode.TwoWay,
    propertyChanging: (bindable, oldValue, newValue) =>
    {
        var control = (DateView)bindable;
        control.titleLabel.Text = (string)newValue;
    });
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(string), typeof(DateView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (DateView)bindable;
            control.valueLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty UnixTimestampValueProperty = BindableProperty.Create(
        nameof(UnixTimestampValue), typeof(long), typeof(DateView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (DateView)bindable;

            var dateTime = DateTime.UnixEpoch.AddSeconds((long)newValue);

            control.valueLabel.Text = dateTime.ToShortDateString();
        });


    public DateView()
	{
		InitializeComponent();
	}
    public string Value
    {
        get => (string)GetValue(ValueProperty);

        set => SetValue(ValueProperty, value);
    }

    public long UnixTimestampValue
    {
        get => (long)GetValue(UnixTimestampValueProperty);

        set => SetValue(UnixTimestampValueProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }
}
