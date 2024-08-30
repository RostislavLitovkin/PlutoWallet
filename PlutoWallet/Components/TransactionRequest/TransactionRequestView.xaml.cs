
using Substrate.NetApi;
using Plutonication;
using Substrate.NetApi.Model.Extrinsics;

namespace PlutoWallet.Components.TransactionRequest;

public partial class TransactionRequestView : ContentView
{
    public TransactionRequestView()
    {
        InitializeComponent();

        BindingContext = DependencyService.Get<TransactionRequestViewModel>();
    }

    private async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }

    private async void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var viewModel = DependencyService.Get<TransactionRequestViewModel>();
            Substrate.NetApi.Model.Extrinsics.Payload payload = viewModel.Payload;

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
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
                        charge: signedExtensions.Charge,
                        checkMetadata: true
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

                await PlutonicationWalletClient.SendPayloadSignatureAsync(signerResult);
            }

            // Hide this layout
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            errorLabel.Text = ex.Message;
            errorLabel.IsVisible = true;
        }
    }

    private async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<TransactionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
