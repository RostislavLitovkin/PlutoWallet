using PlutoWallet.Constants;

namespace PlutoWallet.Components.Xcm;

public partial class XcmNetworkSelectorView : ContentView
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;

            control.chainIcon.Source = (string)newValue;
        });

    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;

            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty WsAddressProperty = BindableProperty.Create(
        nameof(WsAddress), typeof(string), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;

            control.wsAddressLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        nameof(Key), typeof(EndpointEnum), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;
        });

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected), typeof(bool), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;

            if ((bool)newValue)
            {
                control.border.BackgroundColor = Color.FromHex("7aff7a");
            }
            else
            {
                control.border.SetAppThemeColor(Label.BackgroundColorProperty, Color.FromHex("fdfdfd"), Color.FromHex("000000"));
            }
        });

    public static readonly BindableProperty ParachainIdProperty = BindableProperty.Create(
        nameof(ParachainId), typeof(ParachainId), typeof(XcmNetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (XcmNetworkSelectorView)bindable;
        });

    public XcmNetworkSelectorView()
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

    public string WsAddress
    {
        get => (string)GetValue(WsAddressProperty);

        set => SetValue(WsAddressProperty, value);
    }

    public EndpointEnum Key
    {
        get => (EndpointEnum)GetValue(KeyProperty);

        set => SetValue(KeyProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);

        set => SetValue(IsSelectedProperty, value);
    }

    public ParachainId ParachainId
    {
        get => (ParachainId)GetValue(ParachainIdProperty);

        set => SetValue(ParachainIdProperty, value);
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide the AssetSelectView
        var networkSelectPopupViewModel = DependencyService.Get<XcmNetworkSelectPopupViewModel>();

        networkSelectPopupViewModel.SelectEndpoint(Key);
    }
}
