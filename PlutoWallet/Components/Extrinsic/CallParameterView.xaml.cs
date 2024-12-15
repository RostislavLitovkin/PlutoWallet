namespace PlutoWallet.Components.Extrinsic;

public partial class CallParameterView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
       nameof(Name), typeof(string), typeof(CallParameterView),
       defaultBindingMode: BindingMode.TwoWay,
       propertyChanging: (bindable, oldValue, newValue) =>
       {
           var control = (CallParameterView)bindable;

           control.nameLabel.Text = (string)newValue;
       });

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(string), typeof(CallParameterView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (CallParameterView)bindable;

            control.valueLabel.Text = (string)newValue;
        });
    public CallParameterView()
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