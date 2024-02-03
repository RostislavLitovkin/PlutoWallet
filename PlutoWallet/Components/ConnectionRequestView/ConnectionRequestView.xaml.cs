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
                        Console.WriteLine("Payload received: " + payloadJson.ToString());
                        Plutonication.Payload payload = JsonConvert.DeserializeObject<Plutonication.Payload[]>(payloadJson.ToString())[0];

                        Model.PlutonicationModel.ReceivePayload(payload);
                    }
                    catch (Exception ex)
                    {
                        var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                        messagePopup.Title = "ConnectionRequestView Error";
                        messagePopup.Text = ex.Message;

                        messagePopup.IsVisible = true;
                    }
                },
                signRaw: raw =>
                {
                    try
                    {
                        Plutonication.RawMessage message = JsonConvert.DeserializeObject<Plutonication.RawMessage[]>(raw.ToString())[0];

                        if (message.type != "bytes")
                        {
                            throw new Exception("Message is not in bytes format");
                        }

                        var messageSignRequest = DependencyService.Get<MessageSignRequestViewModel>();

                        messageSignRequest.Message = message;
                        messageSignRequest.MessageString = message.data;
                        messageSignRequest.IsVisible = true;
                    }
                    catch (Exception ex)
                    {
                        var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                        messagePopup.Title = "ConnectionRequestView Error";
                        messagePopup.Text = ex.Message;

                        messagePopup.IsVisible = true;
                    }
                }
            );

            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "ConnectionRequestView Error";
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
