using PlutoWallet.Model;
using PlutoWallet.Components.AssetSelect;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;
using PlutoWallet.Types;
using PlutoWallet.Constants;
using ZXing.Aztec.Internal;
using PlutoWallet.Components.TransactionAnalyzer;

namespace PlutoWallet.Components.TransferView;

public partial class TransferView : ContentView
{
	public TransferView()
	{
        var viewModel = DependencyService.Get<TransferViewModel>();

        BindingContext = viewModel;

        InitializeComponent();

        //viewModel.GetFeeAsync();
    }

    async void SignAndTransferClicked(System.Object sender, System.EventArgs e)
    {
        // Send the actual transaction

        var viewModel = DependencyService.Get<TransferViewModel>();
        
        errorLabel.Text = "";

        var clientExt = await Model.AjunaClientModel.GetMainClientAsync();

        var client = clientExt.SubstrateClient;

        try
        {
            var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

            decimal tempAmount;
            BigInteger amount;
            if (decimal.TryParse(viewModel.Amount, out tempAmount))
            {
                // Double to int conversion
                // Complete later

                amount = (BigInteger)(tempAmount * (decimal)Math.Pow(10, assetSelectButtonViewModel.Decimals));

                Console.WriteLine(assetSelectButtonViewModel.Decimals);
            }
            else
            {
                errorLabel.Text = "Invalid amount value";
                return;
            }

            Method transfer = assetSelectButtonViewModel.SelectedAssetKey switch
            {
                (EndpointEnum endpointKey, AssetPallet.Native, _) => TransferModel.NativeTransfer(clientExt, viewModel.Address, amount),
                (EndpointEnum endpointKey, AssetPallet.Assets, BigInteger assetId) => TransferModel.AssetsTransfer(clientExt, viewModel.Address, assetId, amount),
                _ => throw new Exception("Not implemented")
            };
               
            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
            await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, transfer, false, onConfirm: OnConfirmClicked);

            /// Hide this layout

            viewModel.SetToDefault();
            
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
        var viewModel = DependencyService.Get<TransferViewModel>();

        viewModel.SetToDefault();
    }
}
