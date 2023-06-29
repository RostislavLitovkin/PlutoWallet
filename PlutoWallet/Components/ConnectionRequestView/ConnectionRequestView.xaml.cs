using System.Net;
using Substrate.NetApi.Model.Extrinsics;
using Plutonication;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using Schnorrkel;
using Substrate.NetApi;

namespace PlutoWallet.Components.ConnectionRequestView;

public partial class ConnectionRequestView : ContentView
{
	public ConnectionRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ConnectionRequestViewModel>();
    }

    private async void AcceptClicked(System.Object sender, System.EventArgs e)
    {
        try
        {
            var viewModel = DependencyService.Get<ConnectionRequestViewModel>();

            DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
            dAppViewModel.Icon = viewModel.Icon;
            dAppViewModel.Name = viewModel.Name;
            dAppViewModel.IsVisible = true;

            await PlutonicationWalletClient.InitializeAsync(
                ac: viewModel.AccessCredentials,
                pubkey: Model.KeysModel.GetSubstrateKey(),
                signPayload: payload =>
                {
                    try
                    {
                        Console.WriteLine("PAYLOAD: ");
                        Console.WriteLine(payload);

                        byte[] signature = Sr25519v091.SignSimple(Model.KeysModel.GetAccount().Bytes, Model.KeysModel.GetAccount().PrivateKey, payload.InComingBytes[0]);

                        var signerResult = new SignerResult
                        {
                            Id = 0,
                            Signature = Utils.Bytes2HexString(signature),
                        };

                        Console.WriteLine("PAYLOAD sent ");

                        PlutonicationWalletClient.SendSignedPayloadAsync(signerResult);
                        Console.WriteLine("PAYLOAD sent 2");
                    }
                    catch (Exception ex)
                    {
                        var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                        messagePopup.Title = "Error";
                        messagePopup.Text = ex.Message;

                        messagePopup.IsVisible = true;
                    }

                });

            // setup message receive
            //..

            this.IsVisible = false;
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Error";
            messagePopup.Text = ex.Message;

            messagePopup.IsVisible = true;
        }
    }

    private async void RejectClicked(System.Object sender, System.EventArgs e)
    {
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
