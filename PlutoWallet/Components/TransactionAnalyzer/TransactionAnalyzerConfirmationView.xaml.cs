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
        try
        {
            var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
            Payload payload = viewModel.Payload;

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
                Console.WriteLine("Authenticated");

                #region Temp
                var signedExtensions = payload.SignedExtension;

                var tempPayload = new TempPayload(
                    payload.Call,
                    new TempSignedExtensions(
                        specVersion: signedExtensions.SpecVersion,
                        txVersion: signedExtensions.TxVersion,
                        genesis: signedExtensions.Genesis,
                        startEra: signedExtensions.StartEra,
                        mortality: signedExtensions.Mortality,
                        nonce: signedExtensions.Nonce,
                        charge: signedExtensions.Charge
                    )
                );

                #endregion

                byte[] signature = account.Sign(tempPayload.Encode());

                var signerResult = new SignerResult
                {
                    id = 1,
                    signature = Utils.Bytes2HexString(
                        // This 1 means the signature is using Sr25519
                        new byte[1] { 1 }
                        .Concat(signature).ToArray()
                    ).ToLower(),
                };

                Console.WriteLine("Signed");

                await PlutonicationWalletClient.SendPayloadSignatureAsync(signerResult);

                Console.WriteLine("Sending");

            }

            // Hide this layout
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            // Handle potential errors
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        var viewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();
        viewModel.IsVisible = false;
    }
}