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

            viewModel.RequestViewIsVisible = false;
            viewModel.ConnectionStatusIsVisible = true;

            await PlutonicationWalletClient.InitializeAsync(
                ac: viewModel.AccessCredentials,
                pubkey: Model.KeysModel.GetSubstrateKey(),
                signPayload: Model.PlutonicationModel.ReceivePayload,
                signRaw: Model.PlutonicationModel.ReceiveRaw
            );

            // Connection successful

            viewModel.CheckIsVisible = true;
            viewModel.ConnectionStatusText = $"Connected successfully. You can now go back to {viewModel.Name}.";

            DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
            dAppViewModel.Icon = viewModel.Icon;
            dAppViewModel.Name = viewModel.Name;
            dAppViewModel.IsVisible = true;
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

    private async void DismissClicked(System.Object sender, System.EventArgs e)
    {
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
