namespace PlutoWallet.Components.CustomLayouts;

public partial class ExportPlutoLayoutQRView : ContentView
{
	public ExportPlutoLayoutQRView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ExportPlutoLayoutQRViewModel>();

    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<ExportPlutoLayoutQRViewModel>();

        viewModel.IsVisible = false;
    }
}
