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
        public static Dictionary<EndpointEnum, TaskCompletionSource<PlutoWalletSubstrateClient>> Clients = new Dictionary<EndpointEnum, TaskCompletionSource<PlutoWalletSubstrateClient>> ();

        public static async Task<PlutoWalletSubstrateClient> GetMainClientAsync() => await Clients[DependencyService.Get<MultiNetworkSelectViewModel>().SelectedKey.Value].Task;

        public AjunaClientModel()
        {

        }

        public static async Task<PlutoWalletSubstrateClient> GetOrAddSubstrateClientAsync(EndpointEnum endpointKey)
        {
            await ConnectNewSubstrateClientAsync(endpointKey);

            return await Clients[endpointKey].Task;
        }

        public static async Task ConnectNewSubstrateClientAsync(EndpointEnum endpointKey)
        {
            if (Clients.ContainsKey(endpointKey))
            {
                var multiNetworkSelectViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

                // Client is not connected, reconnect it
                if (!await (await Clients[endpointKey].Task).IsConnectedAsync())
                {
                    Console.WriteLine(endpointKey + " was not connected successfully");

                    await (await Clients[endpointKey].Task).ConnectAndLoadMetadataAsync();

                    Console.WriteLine(endpointKey + " now connected?");

                }

                return;
            }

            Console.WriteLine("That " + endpointKey + " was not included");

            Clients.Add(endpointKey, new TaskCompletionSource<PlutoWalletSubstrateClient>());

            Console.WriteLine("now included :)");

            Endpoint endpoint = EndpointsModel.GetEndpoint(endpointKey);

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            Console.WriteLine("Best WebSocket: " + bestWebSecket);

            var client = new PlutoWalletSubstrateClient(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            Clients[endpointKey].SetResult(client);

            Console.WriteLine("Client set");

            await client.ConnectAndLoadMetadataAsync();
        }

        public static async Task RemoveAndDisconnectSubstrateClientAsync(EndpointEnum endpointKey)
        {
            if (!Clients.ContainsKey(endpointKey))
            {
                return;
            }

            (await Clients[endpointKey].Task).Disconnect();

            Clients.Remove(endpointKey);
        }

        /**
         * A Method that assures that when a chain group is changed, all views associated also update.
         * 
         * This method is called in MultiNetworkSelectView.xaml.cs (even during the initialization),
         * so you do not have to worry about not having a chain set up.
         */
        public static async Task ChangeChainGroupAsync(IEnumerable<EndpointEnum> endpointKeys)
        {
            #region Check for updates
            var updateViewModel = DependencyService.Get<UpdateViewModel>();

            try
            {
                await updateViewModel.CheckLatestVersionAsync();
            }
            catch
            {
                // Do nothing.
            }
            #endregion

            var multiNetworkSelectViewModel = DependencyService.Get<MultiNetworkSelectViewModel>();

            #region Remove clients that are not used anymore
            // Remove keys that are not present anymore
            foreach (var key in Clients.Keys)
            {
                if (!endpointKeys.Contains(key))
                {
                    await RemoveAndDisconnectSubstrateClientAsync(key);
                }
            }
            #endregion

            #region Connect clients (one by one)
            foreach (var endpointKey in endpointKeys)
            {
                Console.WriteLine("Trying to connect: " +  endpointKey);
                await ConnectNewSubstrateClientAsync(endpointKey);
                Console.WriteLine("Connected to " + endpointKey.ToString());
            }
            #endregion

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
        public static async Task ChangeChainAsync(EndpointEnum endpointKey)
        {
            await ConnectClientAsync();
        }

        private static async Task ConnectGroupAsync()
        {
            #region Balance view
            var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

            try
            {
                usdBalanceViewModel.UsdSum = "Loading";

                usdBalanceViewModel.ReloadIsVisible = false;

                foreach (var client in Clients.Values)
                {
                    await Model.AssetsModel.GetBalanceAsync(await client.Task, KeysModel.GetSubstrateKey());

                    usdBalanceViewModel.UpdateBalances();
                }

                usdBalanceViewModel.ReloadIsVisible = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Assets error");
                Console.WriteLine(ex);

                usdBalanceViewModel.UsdSum = "Failed";
            }
            #endregion

            #region Hydration
            if (!Clients.ContainsKey(EndpointEnum.Hydration))
            {
                await GetOrAddSubstrateClientAsync(EndpointEnum.Hydration);
            }


            await Model.HydraDX.Sdk.GetAssets((Hydration.NetApi.Generated.SubstrateClientExt)(await Clients[EndpointEnum.Hydration].Task).SubstrateClient, CancellationToken.None);
            Model.AssetsModel.GetUsdBalance();

            usdBalanceViewModel.UpdateBalances();

            var assetInputViewModel = DependencyService.Get<AssetInputViewModel>();
            assetInputViewModel.CalculateUsdValue();

            // Other hydration stuff :)
            var omnipoolLiquidityViewModel = DependencyService.Get<OmnipoolLiquidityViewModel>();

            await omnipoolLiquidityViewModel.GetLiquidityAmount((Hydration.NetApi.Generated.SubstrateClientExt)(await Clients[EndpointEnum.Hydration].Task).SubstrateClient);

            var dcaViewModel = DependencyService.Get<DCAViewModel>();

            await dcaViewModel.GetDCAPosition((Hydration.NetApi.Generated.SubstrateClientExt)(await Clients[EndpointEnum.Hydration].Task).SubstrateClient);
            #endregion

            if (Clients.ContainsKey(EndpointEnum.AzeroTestnet))
            {
                var azeroPrimaryNameViewModel = DependencyService.Get<AzeroPrimaryNameViewModel>();

                await azeroPrimaryNameViewModel.GetPrimaryName(await Clients[EndpointEnum.AzeroTestnet].Task);
            }

            if (Clients.ContainsKey(EndpointEnum.Bifrost))
            {
                var vDotTokenViewModel = DependencyService.Get<VDotTokenViewModel>();

                await vDotTokenViewModel.GetConversionRate((Bifrost.NetApi.Generated.SubstrateClientExt)(await Clients[EndpointEnum.Bifrost].Task).SubstrateClient, CancellationToken.None);
            }

            if (Clients.ContainsKey(EndpointEnum.PolkadotPeople))
            {
                var identityViewModel = DependencyService.Get<IdentityViewModel>();

                await identityViewModel.GetIdentity((PolkadotPeople.NetApi.Generated.SubstrateClientExt)(await Clients[EndpointEnum.PolkadotPeople].Task).SubstrateClient);
            }
        }

        private static async Task ConnectClientAsync()
        {
            var mainClient = await GetMainClientAsync();

            // Setup the AssetSelectButton
            // Do not change it, if the TransferView is visible
            if (!DependencyService.Get<TransferViewModel>().IsVisible)
            {
                var selectedEndpoint = mainClient.Endpoint;

                var assetSelectButtonViewModel = DependencyService.Get<AssetSelectButtonViewModel>();

                assetSelectButtonViewModel.ChainIcon = Application.Current.UserAppTheme == AppTheme.Light ? selectedEndpoint.Icon : selectedEndpoint.DarkIcon;
                assetSelectButtonViewModel.Symbol = selectedEndpoint.Unit;
                assetSelectButtonViewModel.SelectedAssetKey = (selectedEndpoint.Key, AssetPallet.Native, 0);
                assetSelectButtonViewModel.Decimals = selectedEndpoint.Decimals;
            }

            if (!await mainClient.IsConnectedAsync())
            {
                return;
            }

            var chainAddressViewModel = DependencyService.Get<ChainAddressViewModel>();
            var calamarViewModel = DependencyService.Get<CalamarViewModel>();

            var referendaViewModel = DependencyService.Get<ReferendaViewModel>();

            chainAddressViewModel.SetChainAddress(mainClient);
            calamarViewModel.Reload(mainClient.Endpoint);

            await referendaViewModel.GetReferenda(CancellationToken.None);
        }
    }
}

