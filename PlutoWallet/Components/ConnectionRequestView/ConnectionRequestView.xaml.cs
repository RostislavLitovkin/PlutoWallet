using System.Net;
using Ajuna.NetApi.Model.Extrinsics;
using Plutonication;
using PlutoWallet.ViewModel;

namespace PlutoWallet.Components.ConnectionRequestView;

public partial class ConnectionRequestView : ContentView
{
	public ConnectionRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ConnectionRequestViewModel>();
    }

    /*public string IconUrl
    {
        set
        {
            icon.Source = value;
        }
    }
    public string DAppName
    {
        set
        {
            dAppNameLabel.Text = value;
        }
    }
    */

    async void AcceptClicked(System.Object sender, System.EventArgs e)
    {
        var mainViewModel = DependencyService.Get<MainViewModel>();
        mainViewModel.MetadataLabel = "connection accepted";
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();

        var addressPort = viewModel.Url.Split(":");


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

                    Method method = msg.GetMethod();
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

        Model.PlutonicationModel.EventManager.SetupReceiveLoopAsync();


        /*
        Model.PlutonicationModel.Manager = new ClientPlutoManager(IPAddress.Parse(addressPort[0]), Int32.Parse(addressPort[1]));
        Model.PlutonicationModel.Manager.Connect();
        PlutoMessage msg = new PlutoMessage(MessageCode.PublicKey, Model.KeysModel.GetSubstrateKey());
        await Model.PlutonicationModel.Manager.SendMessageAsync(msg);


        mainViewModel.DAppName = "SENT";
        await mainViewModel.PlutonicationLoop();

        */
        //var res = plutonicationManager.ReceiveMessage();
        //Model.PlutonicationModel.Manager.CloseConnection();



        this.IsVisible = false;
    }

    async void RejectClicked(System.Object sender, System.EventArgs e)
    {
        //await this.FadeTo(0, 500);
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
