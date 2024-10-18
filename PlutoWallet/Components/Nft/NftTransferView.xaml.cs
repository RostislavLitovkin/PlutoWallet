using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft;

public partial class NftTransferView : ContentView
{
    public NftTransferView()
	{
		InitializeComponent();

        var viewModel = DependencyService.Get<NftTransferViewModel>();

        BindingContext = viewModel;
    }

    async void SignAndTransferClicked(System.Object sender, System.EventArgs e)
    {
        // Send the actual transaction

        var viewModel = DependencyService.Get<NftTransferViewModel>();

        errorLabel.Text = "";

        var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(viewModel.EndpointKey);

        var client = clientExt.SubstrateClient;

        try
        {
            Method transfer = ((INftTransferable)viewModel.NftBase).Transfer(viewModel.Address);

            /// Hide this layout
            viewModel.SetToDefault();

            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, transfer, false, onConfirm: OnConfirmClicked);
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
            Console.WriteLine(ex);
        }
    }

    public static async Task OnConfirmClicked()
    {
        if ((await KeysModel.GetAccount()).IsSome(out var account))
        {
            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(transactionAnalyzerConfirmationViewModel.Endpoint.Key);

            try
            {
                string extrinsicId = await clientExt.SubmitExtrinsicAsync(transactionAnalyzerConfirmationViewModel.Payload.Call, account, token: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed at confirm clicked");
                Console.WriteLine(ex);
            }

            /// Hide

            transactionAnalyzerConfirmationViewModel.IsVisible = false;
        }
        else
        {
            // Verification failed, do something about it
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        var viewModel = DependencyService.Get<NftTransferViewModel>();

        viewModel.SetToDefault();
    }
}