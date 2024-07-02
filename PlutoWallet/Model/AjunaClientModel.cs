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
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.HydraDX;
using PlutoWallet.Components.Identity;
using PlutoWallet.Components.Referenda;
using PlutoWallet.Types;
using PlutoWallet.Components.VTokens;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.UpdateView;

namespace PlutoWallet.Model
{
    public class AjunaClientModel
    {
        public static PlutoWalletSubstrateClient Client;

        public static PlutoWalletSubstrateClient[] GroupClients;

        public static Endpoint[] GroupEndpoints;

        public static string[] GroupEndpointKeys;

        public static Endpoint SelectedEndpoint;

        public AjunaClientModel()
        {

        }

        /**
         * A Method that assures that when a chain group is changed, all views associated also update.
         * 
         * This method is called in MultiNetworkSelectView.xaml.cs (even during the initialization),
         * so you do not have to worry about not having a chain set up.
         */
        public static async Task ChangeChainGroupAsync(string[] endpointKeys)
        {
            var updateViewModel = DependencyService.Get<UpdateViewModel>();

            try
            {
                await updateViewModel.CheckLatestVersionAsync();
            }
            catch
            {
                // Do nothing.
            }

            GroupEndpointKeys = endpointKeys;

            List<PlutoWalletSubstrateClient> groupClientList = new List<PlutoWalletSubstrateClient>(endpointKeys.Length);

            List<Endpoint> groupEndpointsList = new List<Endpoint>(endpointKeys.Length);

            var multiNetworkSelectViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

            for (int i = 0; i < endpointKeys.Length; i++)
            {
                multiNetworkSelectViewModel.NetworkInfos[i].EndpointConnectionStatus = EndpointConnectionStatus.Loading;
            }

            multiNetworkSelectViewModel.UpdateNetworkInfos();


            for (int i = 0; i < endpointKeys.Length; i++)
            {
                Endpoint endpoint = EndpointsModel.GetEndpoint(endpointKeys[i]);

                groupEndpointsList.Add(endpoint);

                try
                {
                    string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

                    var client = new PlutoWalletSubstrateClient(
                            endpoint,
                            new Uri(bestWebSecket),
                            Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                    await client.ConnectAndLoadMetadataAsync();

                    multiNetworkSelectViewModel.NetworkInfos[i].EndpointConnectionStatus = EndpointConnectionStatus.Connected;

                    multiNetworkSelectViewModel.UpdateNetworkInfos();

                    groupClientList.Add(client);
                }
                catch
                {
                    multiNetworkSelectViewModel.NetworkInfos[i].EndpointConnectionStatus = EndpointConnectionStatus.Failed;

                    multiNetworkSelectViewModel.UpdateNetworkInfos();

                    groupClientList.Add(null);
                }
            }

            GroupClients = groupClientList.ToArray();
            GroupEndpoints = groupEndpointsList.ToArray();

            // Set the first endpoint of the group to be the "main" client
            Client = GroupClients[0];
            SelectedEndpoint = GroupEndpoints[0];
            
            try
            {
                await ConnectClientAsync();
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "AjunaClientModel ConnectClientAsync Error";
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }
            try
            {
                await ConnectGroupAsync();
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "AjunaClientModel ConnectGroupAsync Error";
                Console.WriteLine("AjunaClientModel Error: " + ex);
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }

        }

        /**
         * A Method that assures that when a chain is changed, all views associated also update.
         */
        public static async Task ChangeChainAsync(string endpointKey)
        {
            int index = Array.IndexOf(GroupEndpointKeys, endpointKey);
            SelectedEndpoint = GroupEndpoints[index];

            // Set the selected endpoint of the group to be the "main" client
            Client = GroupClients[index];

            await ConnectClientAsync();
        }

        /**
         * A Method that assures that when a chain is changed, all views associated also update.
         */
        public static async Task ChangeChainAsync(Endpoint endpoint)
        {
            int index = Array.IndexOf(GroupEndpoints, endpoint);
            SelectedEndpoint = GroupEndpoints[index];

            // Set the selected endpoint of the group to be the "main" client
            Client = GroupClients[index];

            await ConnectClientAsync();
        }

        private static async Task ConnectGroupAsync()
        {
            // Wait up to 10 seconds for all clients to connect. 
            /*for (int i = 0; i < 20; i++)
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
            }*/

            // Setup things, like balances..

            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

            try
            {
                usdBalanceViewModel.UsdSum = "Loading";

                usdBalanceViewModel.ReloadIsVisible = false;

                await Model.AssetsModel.GetBalance(Model.AjunaClientModel.GroupClients, Model.AjunaClientModel.GroupEndpoints, KeysModel.GetSubstrateKey());
            }
            catch (Exception ex)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Loading Assets Error";
                messagePopup.Text = ex.Message;
                Console.WriteLine("Assets error");
                Console.WriteLine(ex);


                messagePopup.IsVisible = true;

                usdBalanceViewModel.UsdSum = "Failed";

                return;
            }

            usdBalanceViewModel.UpdateBalances();

            bool hydraClientNotFound = true;

            for (int i = 0; i < GroupEndpoints.Length; i++)
            {
                if (GroupEndpoints[i].Key == "hydradx")
                {
                    await Model.HydraDX.Sdk.GetAssets(GroupClients[i], CancellationToken.None);
                    Model.AssetsModel.GetUsdBalance();

                    usdBalanceViewModel.UpdateBalances();

                    hydraClientNotFound = false;

                    // Other hydration stuff :)
                    var omnipoolLiquidityViewModel = DependencyService.Get<OmnipoolLiquidityViewModel>();

                    await omnipoolLiquidityViewModel.GetLiquidityAmount(GroupClients[i]);

                    var dcaViewModel = DependencyService.Get<DCAViewModel>();

                    await dcaViewModel.GetDCAPosition(GroupClients[i]);
                }
            }

            if (hydraClientNotFound)
            {
                try
                {
                    Endpoint hdxEndpoint = Endpoints.GetEndpointDictionary["hydradx"];
                    string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(hdxEndpoint.URLs);

                    var client = new PlutoWalletSubstrateClient(
                                hdxEndpoint,
                                new Uri(bestWebSecket),
                                Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                    await client.ConnectAndLoadMetadataAsync();

                    await Model.HydraDX.Sdk.GetAssets(client, CancellationToken.None);
                    Model.AssetsModel.GetUsdBalance();
                }
                catch
                {

                }
            }

            for (int i = 0; i < GroupEndpoints.Length; i++)
            {
                if (GroupEndpoints[i].Name == "Aleph Zero Testnet")
                {
                    var azeroPrimaryNameViewModel = DependencyService.Get<AzeroPrimaryNameViewModel>();

                    await azeroPrimaryNameViewModel.GetPrimaryName(GroupClients[i]);
                }

                if (GroupEndpoints[i].Key == "bifrost")
                {
                    var vDotTokenViewModel = DependencyService.Get<VDotTokenViewModel>();

                    await vDotTokenViewModel.GetConversionRate(GroupClients[i], CancellationToken.None);
                }
            }
        }

        private static async Task ConnectClientAsync()
        {
            // Setup the AssetSelectButton
            // Do not change it, if the TransferView is visible
            if (!DependencyService.Get<TransferViewModel>().IsVisible)
            {
                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

                assetSelectButtonViewModel.ChainIcon = Application.Current.UserAppTheme == AppTheme.Light ? SelectedEndpoint.Icon : SelectedEndpoint.DarkIcon;
                assetSelectButtonViewModel.Symbol = SelectedEndpoint.Unit;
                assetSelectButtonViewModel.AssetId = 0;
                assetSelectButtonViewModel.Pallet = AssetPallet.Native;
                assetSelectButtonViewModel.Endpoint = SelectedEndpoint;
                assetSelectButtonViewModel.Decimals = SelectedEndpoint.Decimals;
            }

            // Wait up to 10 seconds for the Client to connect. 
            /*for (int i = 0; i < 20; i++)
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
            }*/

            // task completed within timeout

            // Setup things, like balances..
            //var customCallsViewModel = DependencyService.Get<CustomCallsViewModel>();
            var mainViewModel = DependencyService.Get<MainViewModel>();
            var transferViewModel = DependencyService.Get<TransferViewModel>();
            var chainAddressViewModel = DependencyService.Get<ChainAddressViewModel>();
            var calamarViewModel = DependencyService.Get<CalamarViewModel>();

            var identityViewModel = DependencyService.Get<IdentityViewModel>();
            var referendaViewModel = DependencyService.Get<ReferendaViewModel>();

            chainAddressViewModel.SetChainAddress();
            calamarViewModel.Reload();
            await identityViewModel.GetIdentity();

            await referendaViewModel.GetReferenda();
            //customCallsViewModel.GetMetadata();

            if (Client is null)
            {
                return;
            }

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
        }
    }
}

