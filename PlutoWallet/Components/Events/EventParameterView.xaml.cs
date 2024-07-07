namespace PlutoWallet.Components.Events;

public partial class EventParameterView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(EventParameterView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventParameterView)bindable;

            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(string), typeof(EventParameterView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (EventParameterView)bindable;

            control.valueLabel.Text = (string)newValue;
        });
    public EventParameterView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}