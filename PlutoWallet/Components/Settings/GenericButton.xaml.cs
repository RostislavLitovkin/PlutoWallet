namespace PlutoWallet.Components.Settings;

public partial class GenericButton : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(GenericButton),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (GenericButton)bindable;
            control.nameLabelText.Text = (string)newValue;
        });

    public GenericButton()
	{
		InitializeComponent();
	}

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }
}
