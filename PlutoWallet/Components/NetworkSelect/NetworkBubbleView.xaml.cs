using Microsoft.Maui.Controls;
using PlutoWallet.Constants;
using PlutoWallet.Model;

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

            if ((string)newValue == "menuwhite.png")
            {
                control.iconImage.SetAppTheme<FileImageSource>(Image.SourceProperty, "menuwhite.png", "menublack.png");
                control.iconImage.WidthRequest = 24;
                control.iconImage.HeightRequest = 24;
                control.iconImage.Margin = 3;
            }
        });

    public static readonly BindableProperty ShowNameProperty = BindableProperty.Create(
        nameof(ShowName), typeof(bool), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;
            control.nameLabel.IsVisible = (bool)newValue;
        });

    public static readonly BindableProperty EndpointKeyProperty = BindableProperty.Create(
        nameof(EndpointKey), typeof(EndpointEnum), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;

            if ((EndpointEnum)newValue == EndpointEnum.None)
            {
                return;
            }

            Endpoint endpoint = EndpointsModel.GetEndpoint((EndpointEnum)newValue);

            control.iconImage.SetAppTheme<FileImageSource>(Image.SourceProperty, endpoint.DarkIcon, endpoint.Icon);
        });

    public static readonly BindableProperty EndpointConnectionStatusProperty = BindableProperty.Create(
        nameof(EndpointConnectionStatus), typeof(EndpointConnectionStatus), typeof(NetworkBubbleView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkBubbleView)bindable;

            switch ((EndpointConnectionStatus)newValue) {
                case EndpointConnectionStatus.Loading:
                    control.connectionStatusBorder.BackgroundColor = Colors.RoyalBlue;
                    control.connectionStatusIcon.Source = "loadingdotswhite.png";

                    break;

                case EndpointConnectionStatus.Connected:
                    control.connectionStatusBorder.BackgroundColor = Colors.Green;
                    control.connectionStatusIcon.Source = "connectedwhite.png";

                    break;

                case EndpointConnectionStatus.Failed:
                    control.connectionStatusBorder.BackgroundColor = Colors.DarkRed;
                    control.connectionStatusIcon.Source = "disconnectedwhite.png";

                    break;
                case EndpointConnectionStatus.None:
                    control.connectionStatusBorder.BackgroundColor = Color.FromArgb("#00000000");
                    control.connectionStatusIcon.Source = "";

                    break;
            }
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

    public EndpointEnum EndpointKey
    {
        get => (EndpointEnum)GetValue(EndpointKeyProperty);
        set => SetValue(EndpointKeyProperty, value);
    }

    public EndpointConnectionStatus EndpointConnectionStatus
    {
        get => (EndpointConnectionStatus)GetValue(EndpointConnectionStatusProperty);
        set => SetValue(EndpointConnectionStatusProperty, value);
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
