namespace PlutoWallet.Components.Buttons;

public partial class BasicGrayButton : Button
{
    public static readonly BindableProperty ButtonStateProperty = BindableProperty.Create(
        nameof(ButtonState), typeof(ButtonStateEnum), typeof(BasicGrayButton),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (BasicGrayButton)bindable;

            switch ((ButtonStateEnum)newValue)
            {
                case ButtonStateEnum.Enabled:
                    if (App.Current.Resources.TryGetValue("PrimaryGray", out object primaryGrayColor))
                    {
                        control.BackgroundColor = (Color)primaryGrayColor;
                    }

                    control.TextColor = Colors.White;

                    control.IsEnabled = true;
                    break;
                case ButtonStateEnum.Disabled:
                    if (App.Current.Resources.TryGetValue("PrimaryGrayUnimportant", out object primaryGrayUnimportantColor))
                    {
                        control.BackgroundColor = (Color)primaryGrayUnimportantColor;
                    }

                    control.IsEnabled = false;
                    break;
                case ButtonStateEnum.Warning:
                    control.IsEnabled = true;
                    control.BackgroundColor = Colors.Red;
                    control.TextColor = Colors.White;
                    break;
                case ButtonStateEnum.Invisible:
                    control.IsVisible = false;
                    break;
            }
        },
        defaultValue: ButtonStateEnum.Enabled);

    public BasicGrayButton()
	{
		InitializeComponent();
	}

    public ButtonStateEnum ButtonState
    {
        get => (ButtonStateEnum)GetValue(ButtonStateProperty);
        set => SetValue(ButtonStateProperty, value);
    }
}