using System.Net;
using Substrate.NetApi.Model.Extrinsics;
using Plutonication;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.MessagePopup;
using Schnorrkel;
using Substrate.NetApi;
using Newtonsoft.Json;
using PlutoWallet.Components.TransactionRequest;

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
                signPayload: payloadJson =>
                {
                    try
                    {
                        Plutonication.Payload payload = JsonConvert.DeserializeObject<Plutonication.Payload[]>(payloadJson.ToString())[0];

                        byte[] methodBytes = Utils.HexToByteArray(payload.method);

                        List<byte> methodParameters = new List<byte>();

                        for (int i = 2; i < methodBytes.Length; i++)
                        {
                            methodParameters.Add(methodBytes[i]);
                        }

                        Method method = new Method(methodBytes[0], methodBytes[1], methodParameters.ToArray());

                        var transactionRequest = DependencyService.Get<TransactionRequestViewModel>();

                        transactionRequest.AjunaMethod = method;
                        transactionRequest.Payload = payload;
                        transactionRequest.IsVisible = true;

                        //byte[] signature = Sr25519v091.SignSimple(Model.KeysModel.GetAccount().Bytes, Model.KeysModel.GetAccount().PrivateKey, payload.InComingBytes[0]);

                        /*
                        var signerResult = new SignerResult
                        {
                            id = 0,
                            signature = Utils.Bytes2HexString(signature),
                        };

                        PlutonicationWalletClient.SendSignedPayloadAsync(signerResult);
                        */
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

            viewModel.IsVisible = false;
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
