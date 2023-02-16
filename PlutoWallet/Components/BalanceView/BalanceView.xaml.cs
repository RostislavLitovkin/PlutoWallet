using PlutoWallet.Components.TransferView;

namespace PlutoWallet.Components.BalanceView;

public partial class BalanceView : ContentView
{
	
	public BalanceView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<BalanceViewModel>();
	}

    async void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BalanceViewModel>();
        await viewModel.GetBalanceAsync();
    }

    void OnTransferClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<TransferViewModel>();
        viewModel.IsVisible = true;

    }
}
