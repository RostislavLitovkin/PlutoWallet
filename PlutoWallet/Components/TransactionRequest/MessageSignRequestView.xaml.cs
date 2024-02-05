using Substrate.NetApi;
using Plutonication;
using Substrate.NetApi.Model.Types;
using Chaos.NaCl;
using Schnorrkel;

namespace PlutoWallet.Components.TransactionRequest;

public partial class MessageSignRequestView : ContentView
{
	public MessageSignRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<MessageSignRequestViewModel>();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Maybe send a refuse message 

        // Hide this layout
        var viewModel = DependencyService.Get<MessageSignRequestViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var viewModel = DependencyService.Get<MessageSignRequestViewModel>();
            byte[] msg = Utils.HexToByteArray(viewModel.Message.data);

            if ((await Model.KeysModel.GetAccount()).IsSome(out var account))
            {
                if (msg.Length > 256) msg = HashExtension.Blake2(msg, 256);

                byte[] signature;
                switch (account.KeyType)
                {
                    case KeyType.Ed25519:
                        signature = Ed25519.Sign(msg, account.PrivateKey);
                        break;

                    case KeyType.Sr25519:
                        signature = Sr25519v091.SignSimple(account.Bytes, account.PrivateKey, msg);
                        break;

                    default:
                        throw new Exception($"Unknown key type found '{account.KeyType}'.");
                }

                var signerResult = new SignerResult
                {
                    id = 1,
                    signature = Utils.Bytes2HexString(signature).ToLower(),
                };

                await PlutonicationWalletClient.SendRawSignatureAsync(signerResult);
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
        var viewModel = DependencyService.Get<MessageSignRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
