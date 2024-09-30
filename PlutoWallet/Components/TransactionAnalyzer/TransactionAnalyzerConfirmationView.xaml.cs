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
            await viewModel.OnConfirm();
        }

        viewModel.SetToDefault();
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
            var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
            viewModel.SetToDefault();
    }
}