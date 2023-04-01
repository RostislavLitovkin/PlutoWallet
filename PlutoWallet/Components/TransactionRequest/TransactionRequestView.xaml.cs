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
        await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Refused);

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

            await client.Author.SubmitExtrinsicAsync(
                viewModel.AjunaMethod,
                Model.KeysModel.GetAccount(),
                Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default(),
                64
             );

            // Tell the dApp that the transaction was successfull
            await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Success);

            // Hide this layout
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
        }

    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        await Model.PlutonicationModel.EventManager.SendMessageAsync(MessageCode.Refused);

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
