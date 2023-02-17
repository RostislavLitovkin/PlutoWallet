namespace PlutoWallet.Components.PublicKeyQRCodeView;

public partial class PublicKeyQRCodeView : ContentView
{
	public PublicKeyQRCodeView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<PublicKeyQRCodeViewModel>();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<PublicKeyQRCodeViewModel>();

        viewModel.IsVisible = false;
    }
}
