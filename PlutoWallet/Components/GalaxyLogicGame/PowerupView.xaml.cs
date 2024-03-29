namespace PlutoWallet.Components.GalaxyLogicGame;

public partial class PowerupView : ContentView
{

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(PowerupView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (PowerupView)bindable;

            control.image.Source = (string)newValue;
        });

    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(PowerupView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (PowerupView)bindable;

            control.nameLabel.Text = (string)newValue;
        });

    public PowerupView()
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
}
