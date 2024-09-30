namespace PlutoWallet.Components.Buttons;

public partial class ElevatedButton : Button
{
    public static readonly BindableProperty ButtonStateProperty = BindableProperty.Create(
        nameof(ButtonState), typeof(ButtonStateEnum), typeof(ElevatedButton),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (ElevatedButton)bindable;

            switch ((ButtonStateEnum)newValue)
            {
                case ButtonStateEnum.Enabled:
                    if (App.Current.Resources.TryGetValue("Primary", out object primaryColor))
                    {
                        control.BackgroundColor = (Color)primaryColor;
                    }

                    control.TextColor = Colors.White;

                    control.IsEnabled = true;
                    break;
                case ButtonStateEnum.Disabled:
                    if (App.Current.Resources.TryGetValue("PrimaryUnimportant", out object primaryUnimportantColor))
                    {
                        control.BackgroundColor = (Color)primaryUnimportantColor;
                    }

                    control.IsEnabled = false;
                    break;
                case ButtonStateEnum.Warning:
                    control.IsEnabled = true;
                    control.BackgroundColor = Colors.Red;
                    control.TextColor = Colors.White;
                    break;
            }
        },
        defaultValue: ButtonStateEnum.Enabled);

    public ElevatedButton()
	{
		InitializeComponent();
	}

    public ButtonStateEnum ButtonState
    {
        get => (ButtonStateEnum)GetValue(ButtonStateProperty);
        set => SetValue(ButtonStateProperty, value);
    }
}