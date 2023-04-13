using System;
using Plutonication;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Constants;
using PlutoWallet.Components.AddressView;

namespace PlutoWallet.Model
{
	public class AjunaClientModel
	{
        public static AjunaClientExt Client;

        public static Endpoint SelectedEndpoint;

        public static bool Connected = false;

        public AjunaClientModel()
		{
            
        }

        /**
         * A Method that assures that when a chain is changed, all views associated also update.
         * 
         * This method is called in MultiNetworkSelectView.xaml.cs (even during the initialization),
         * so you do not have to worry about not having a chain set up.
         */
        public static async Task ChangeChainAsync(int endpointIndex)
        {
            SelectedEndpoint = Endpoints.GetAllEndpoints[endpointIndex];
             
            Client = new AjunaClientExt(
                    new Uri(SelectedEndpoint.URL),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await ConnectAsync();
        }

        private static async Task ConnectAsync()
        {
            Connected = false;

            Task connectAsync = Client.ConnectAsync();

            int timeout = 10000;
            if (await Task.WhenAny(connectAsync, Task.Delay(timeout)) == connectAsync)
            {
                // task completed within timeout
                Connected = true;

                // Setup things, like balances..
                var customCallsViewModel = DependencyService.Get<ViewModel.CustomCallsViewModel>();
                var mainViewModel = DependencyService.Get<ViewModel.MainViewModel>();
                var balanceViewModel = DependencyService.Get<Components.Balance.BalanceViewModel>();
                var transferViewModel = DependencyService.Get<Components.TransferView.TransferViewModel>();
                var chainAddressViewModel = DependencyService.Get<ChainAddressViewModel>();

                chainAddressViewModel.SetChainAddress();
                customCallsViewModel.GetMetadata();
                Task getBalance = balanceViewModel.GetBalanceAsync();
                Task getTransfer = transferViewModel.GetFeeAsync();
            }
            else
            {
                // timeout logic
            }
        }
	}
}

