namespace PlutoWallet.Components.AddressView;

public partial class AddressQrCodeView : ContentView
{
	public AddressQrCodeView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<AddressQrCodeViewModel>();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<AddressQrCodeViewModel>();

        viewModel.IsVisible = false;
    }

    private async Task CopyImageAnimation()
    {
        copyImage.Source = "greentick.png";

        await Task.Delay(3000);

        copyImage.SetAppTheme<FileImageSource>(Image.SourceProperty, "copyblack.png", "copywhite.png");
    }

    private async void OnCopyAddressClicked(object sender, TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<AddressQrCodeViewModel>();

        CopyImageAnimation();

		await CopyAddress.CopyToClipboardAsync(viewModel.Address);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}