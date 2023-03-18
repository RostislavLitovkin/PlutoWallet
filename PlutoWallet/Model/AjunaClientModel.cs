using System;
using Plutonication;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model
{
	public class AjunaClientModel
	{
        public static AjunaClientExt Client;

        public static bool Connected = false;

        public AjunaClientModel()
		{
            Client = new AjunaClientExt(
                    new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")),
                    Ajuna.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            Task connectTask = ConnectAsync();
        }

        public static async Task ChangeChainAsync()
        {
            Client = new AjunaClientExt(
                    new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")),
                    Ajuna.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

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
                var balanceViewModel = DependencyService.Get<Components.BalanceView.BalanceViewModel>();

                customCallsViewModel.GetMetadata();
                Task gettingBalance = balanceViewModel.GetBalanceAsync();
            }
            else
            {
                // timeout logic
            }
        }
	}
}

