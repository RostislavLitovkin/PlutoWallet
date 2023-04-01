using System.Net;
using Substrate.NetApi.Model.Extrinsics;
using Plutonication;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.DAppConnectionView;

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
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();

        var addressPort = viewModel.Url.Split(":");

        DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
        dAppViewModel.Icon = viewModel.Icon;
        dAppViewModel.Name = viewModel.Name;
        dAppViewModel.IsVisible = true;

        Model.PlutonicationModel.EventManager = new PlutoEventManager();

        Model.PlutonicationModel.EventManager.ConnectionEstablished += () =>
        {
            Console.WriteLine("Connectin Established! :D");
        };
        Model.PlutonicationModel.EventManager.ConnectionRefused += () =>
        {
            Console.WriteLine("Connectin Refused! :(");
        };

        AccessCredentials accessCredentials = new AccessCredentials(IPAddress.Parse(addressPort[0]), Int32.Parse(addressPort[1]));
        accessCredentials.Key = viewModel.Key;
        await Model.PlutonicationModel.EventManager.ConnectSafeAsync(accessCredentials);

        await Task.Delay(1000);
        
        PlutoMessage msg = new PlutoMessage(MessageCode.PublicKey, Model.KeysModel.GetSubstrateKey());

        await Model.PlutonicationModel.EventManager.SendMessageAsync(msg);

        Model.PlutonicationModel.EventManager.MessageReceived += () =>
        {
            PlutoMessage msg = Model.PlutonicationModel.EventManager.IncomingMessages.Dequeue();

            switch(msg.Identifier)
            {
                case MessageCode.Method:

                    // This is temporary
                    Ajuna.NetApi.Model.Extrinsics.Method tempMethod = msg.GetMethod();

                    Method method = new Method(tempMethod.ModuleIndex, tempMethod.CallIndex, tempMethod.Parameters);

                    var transactionRequestViewModel = DependencyService.Get<Components.TransactionRequest.TransactionRequestViewModel>();
                    transactionRequestViewModel.AjunaMethod = method;
                    transactionRequestViewModel.IsVisible = true;

                    break;

                default:

                    break;
            }
            Console.WriteLine("Code: " + msg.Identifier);
            Console.WriteLine("Data: " + msg.CustomDataToString());
        };

        Task setup = Model.PlutonicationModel.EventManager.SetupReceiveLoopAsync();

        this.IsVisible = false;
    }

    private async void RejectClicked(System.Object sender, System.EventArgs e)
    {
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
