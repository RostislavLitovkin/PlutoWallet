namespace PlutoWallet.Components.CalamarView;

public partial class CalamarView : ContentView
{
    public CalamarView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<CalamarViewModel>();
    }

    void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<CalamarViewModel>();
        viewModel.Reload();
    }

    async void OnOpenClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<CalamarViewModel>();
        await Launcher.OpenAsync(viewModel.WebAddress);
    }
}
