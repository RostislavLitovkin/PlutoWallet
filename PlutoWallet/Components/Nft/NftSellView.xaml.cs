using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft;

public partial class NftSellView : ContentView
{
	public NftSellView()
	{
		InitializeComponent();

        var viewModel = DependencyService.Get<NftSellViewModel>();

        BindingContext = viewModel;
    }

    async void SignAndSellClicked(System.Object sender, System.EventArgs e)
    {
        // Send the actual transaction

        var viewModel = DependencyService.Get<NftSellViewModel>();

        errorLabel.Text = "";

        var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(viewModel.Endpoint.Key);

        var client = clientExt.SubstrateClient;

        try
        {
            decimal tempAmount;
            BigInteger amount;
            if (decimal.TryParse(viewModel.Amount, out tempAmount))
            {
                // Double to int conversion
                // Complete later

                amount = (BigInteger)(tempAmount * (decimal)Math.Pow(10, viewModel.Endpoint.Decimals));

                Console.WriteLine(viewModel.Endpoint.Decimals);
            }
            else
            {
                errorLabel.Text = "Invalid amount value";
                return;
            }

            Method sell = ((INftSellable)viewModel.NftBase).Sell(amount);


            /// Hide this layout
            viewModel.SetToDefault();

            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, sell, false, onConfirm: OnConfirmClicked);
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