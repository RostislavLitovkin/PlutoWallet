using System;

namespace PlutoWallet.Components.NetworkSelect;

public partial class NetworkSelectorView : ContentView
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(
        nameof(Icon), typeof(string), typeof(NetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkSelectorView)bindable;

            control.chainIcon.Source = (string)newValue;
        });

    public static readonly BindableProperty NameProperty = BindableProperty.Create(
        nameof(Name), typeof(string), typeof(NetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkSelectorView)bindable;

            control.nameLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty WsAddressProperty = BindableProperty.Create(
        nameof(WsAddress), typeof(string), typeof(NetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkSelectorView)bindable;

            control.wsAddressLabel.Text = (string)newValue;
        });

    public static readonly BindableProperty KeyProperty = BindableProperty.Create(
        nameof(Key), typeof(string), typeof(NetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkSelectorView)bindable;
        });

    public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(
        nameof(IsSelected), typeof(bool), typeof(NetworkSelectorView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (NetworkSelectorView)bindable;

            if ((bool)newValue)
            {
                control.border.BackgroundColor = Color.FromHex("7aff7a");
            }
            else
            {
                control.border.SetAppThemeColor(Label.BackgroundColorProperty, Color.FromHex("fdfdfd"), Color.FromHex("000000"));
            }
        });

    public NetworkSelectorView()
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

    public string Key
    {
        get => (string)GetValue(KeyProperty);

        set => SetValue(KeyProperty, value);
    }

    public bool IsSelected
    {
        get => (bool)GetValue(IsSelectedProperty);

        set => SetValue(IsSelectedProperty, value);
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide the AssetSelectView
        var networkSelectPopupViewModel = DependencyService.Get<NetworkSelectPopupViewModel>();

        Console.WriteLine("Selected:" + Key);

        networkSelectPopupViewModel.SelectEndpoint(Key);
    }
}
