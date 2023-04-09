using Microsoft.Maui.Controls;

namespace PlutoWallet.Components.NetworkSelect;

public partial class NetworkBubbleView : ContentView
{
    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;
            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;
            control.iconImage.Source = (string)newValue;
        });

    public static readonly BindableProperty ShowNameProperty = BindableProperty.Create(
        nameof(ShowName), typeof(bool), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;
            control.nameLabel.IsVisible = (bool)newValue;
        });

    public NetworkBubbleView()
	{
		InitializeComponent();
	}

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool ShowName
    {
        get => (bool)GetValue(ShowNameProperty);
        set => SetValue(ShowNameProperty, value);
    }



    public event EventHandler<TappedEventArgs> Tapped
    {
        add
        {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += value;
            horizontalStackLayout.GestureRecognizers.Add(tapGestureRecognizer);
        }
        remove
        {
            throw new NotImplementedException();
        }
    }
}
