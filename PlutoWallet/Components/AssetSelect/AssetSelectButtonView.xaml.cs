namespace PlutoWallet.Components.AssetSelect;

public partial class AssetSelectButtonView : ContentView
{
	public AssetSelectButtonView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<AssetSelectButtonViewModel>();
    }

    private async void OnChangeTokenClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<AssetSelectViewModel>();

        await viewModel.Appear();
    }
}
