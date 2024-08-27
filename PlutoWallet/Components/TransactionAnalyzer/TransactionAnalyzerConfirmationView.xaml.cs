using Plutonication;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Payload = Substrate.NetApi.Model.Extrinsics.Payload;

namespace PlutoWallet.Components.TransactionAnalyzer;

public partial class TransactionAnalyzerConfirmationView : ContentView
{
	public TransactionAnalyzerConfirmationView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
	}

    private async void OnConfirmClicked(object sender, EventArgs e)
    {
        var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

        if (viewModel.OnConfirm != null)
        {
            Console.WriteLine("Confirming outside");
            await viewModel.OnConfirm();
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
        viewModel.IsVisible = false;
    }
}