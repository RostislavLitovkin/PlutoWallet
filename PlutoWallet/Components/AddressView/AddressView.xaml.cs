using CommunityToolkit.Maui.Alerts;

namespace PlutoWallet.Components.AddressView;

public partial class AddressView : ContentView
{
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(AddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AddressView)bindable;
			control.addressLabel.Text = ((string)newValue).Substring(0, 12) + "..";
        });

    public static readonly BindableProperty QrAddressProperty = BindableProperty.Create(
        nameof(QrAddress), typeof(string), typeof(AddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
        });

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title), typeof(string), typeof(AddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) => {
            var control = (AddressView)bindable;
            control.titleLabel.Text = (string)newValue;
        });

    public AddressView()
	{
		InitializeComponent();
	}

	public string Address
	{
		get => (string)GetValue(AddressProperty);
	
		set => SetValue(AddressProperty, value);
	}

    public string QrAddress
    {
        get => (string)GetValue(QrAddressProperty);

        set => SetValue(QrAddressProperty, value);
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);

        set => SetValue(TitleProperty, value);
    }

    private async void OnTapped(System.Object sender, System.EventArgs e)
    {
		await CopyAddress.CopyToClipboardAsync((string)GetValue(AddressProperty));
    }

    private void OnQRTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		var qrViewModel = DependencyService.Get<AddressQrCodeViewModel>();

		qrViewModel.QrAddress = (string)GetValue(QrAddressProperty);
        qrViewModel.Address = (string)GetValue(AddressProperty);
        qrViewModel.IsVisible = true;
    }
}
