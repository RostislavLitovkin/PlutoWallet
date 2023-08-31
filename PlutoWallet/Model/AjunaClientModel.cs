using System;
using Plutonication;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.ViewModel;
using PlutoWallet.Constants;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.TransferView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.CalamarView;
using PlutoWallet.Components.AzeroId;

namespace PlutoWallet.Model
{
    public class AjunaClientModel
    {
        public static AjunaClientExt Client;

        public static AjunaClientExt[] GroupClients;

        public static Endpoint[] GroupEndpoints;

        private static int[] groupEndpointIndexes;

        public static Endpoint SelectedEndpoint;

        public static bool Connected = false;

        public AjunaClientModel()
        {

        }

        /**
         * A Method that assures that when a chain group is changed, all views associated also update.
         * 
         * This method is called in MultiNetworkSelectView.xaml.cs (even during the initialization),
         * so you do not have to worry about not having a chain set up.
         */
        public static async Task ChangeChainGroupAsync(int[] endpointIndexes)
        {
            try
            {
                groupEndpointIndexes = endpointIndexes;

                List<AjunaClientExt> groupClientList = new List<AjunaClientExt>(endpointIndexes.Length);

                List<Endpoint> groupEndpointsList = new List<Endpoint>(endpointIndexes.Length);

                for (int i = 0; i < endpointIndexes.Length; i++)
                {
                    // check that the index exists
                    if (endpointIndexes[i] != -1)
                    {
                        Endpoint endpoint = Endpoints.GetAllEndpoints[endpointIndexes[i]];

                        groupEndpointsList.Add(endpoint);

                        var client = new AjunaClientExt(
                                new Uri(endpoint.URL),
                                Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                        client.ConnectAsync();

                        groupClientList.Add(client);
                    }
                }

                GroupClients = groupClientList.ToArray();
                GroupEndpoints = groupEndpointsList.ToArray();

                // Set the first endpoint of the group to be the "main" client
                Client = GroupClients[0];
                SelectedEndpoint = GroupEndpoints[0];

                await ConnectClientAsync();
                await ConnectGroupAsync();
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Error";
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }
        }

        /**
         * A Method that assures that when a chain is changed, all views associated also update.
         */
        public static async Task ChangeChainAsync(int endpointIndex)
        {
            int index = Array.IndexOf(groupEndpointIndexes, endpointIndex);
            SelectedEndpoint = GroupEndpoints[index];

            // Set the selected endpoint of the group to be the "main" client
            Client = GroupClients[index];

            await ConnectClientAsync();
        }
    

        private static async Task ConnectGroupAsync()
        {
            // Wait up to 10 seconds for all clients to connect. 
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(500);

                bool allConnected = true;

                foreach (var client in GroupClients)
                {
                    if (!client.IsConnected)
                    {
                        allConnected = false;
                        break;
                    }
                }

                if (allConnected) break;
            }

            // Setup things, like balances..
            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

            Task getBalance = usdBalanceViewModel.GetBalancesAsync();

        }

        private static async Task ConnectClientAsync()
        {
            Connected = false;

            // Wait up to 10 seconds for the Client to connect. 
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(500);

                if (Client.IsConnected) break;
            }

            if (!Client.IsConnected)
            {
                // show unable to connect error message
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Failed to connect";
                messagePopup.Text = "Failed to establish connection to the selected endpoint. " +
                    "Check your internet connection or try a different endpoint.";

                messagePopup.IsVisible = true;

                return;
            }

            // task completed within timeout
            Connected = true;

            // Setup things, like balances..
            //var customCallsViewModel = DependencyService.Get<CustomCallsViewModel>();
            var mainViewModel = DependencyService.Get<MainViewModel>();
            var balanceViewModel = DependencyService.Get<BalanceViewModel>();
            var transferViewModel = DependencyService.Get<TransferViewModel>();
            var chainAddressViewModel = DependencyService.Get<ChainAddressViewModel>();
            var calamarViewModel = DependencyService.Get<CalamarViewModel>();

            if(SelectedEndpoint.Name == "Aleph Zero Testnet")
            {
                var azeroPrimaryNameViewModel = DependencyService.Get<AzeroPrimaryNameViewModel>();

                azeroPrimaryNameViewModel.GetPrimaryName();
            }


            chainAddressViewModel.SetChainAddress();
            calamarViewModel.Reload();
            //customCallsViewModel.GetMetadata();


            // Wait up to 10 seconds for the Client to fetch metadata 
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(500);

                if (Client.MetaData != null) break;
            }

            if (Client.MetaData == null)
            {
                // show unable to connect error message
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Failed to connect";
                messagePopup.Text = "Failed to fetch metadata";

                messagePopup.IsVisible = true;

                return;
            }

            await transferViewModel.GetFeeAsync();

            Console.WriteLine("All done");

        }
    }
}

