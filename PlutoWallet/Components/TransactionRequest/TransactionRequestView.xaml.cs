using Plutonication;

namespace PlutoWallet.Components.TransactionRequest;

public partial class TransactionRequestView : ContentView
{
	public TransactionRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<TransactionRequestViewModel>();
    }
    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {

        try
        {

            var viewModel = DependencyService.Get<TransactionRequestViewModel>();

            var client = Model.AjunaClientModel.Client;

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
                await client.Author.SubmitExtrinsicAsync(
                    viewModel.AjunaMethod,
                    account,
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default(),
                    64
                );
            }

            // Tell the dApp that the transaction was successfull

            // Hide this layout
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
            errorLabel.IsVisible = true;
        }

    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
