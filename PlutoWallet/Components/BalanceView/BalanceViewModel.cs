using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.BalanceView
{
	public partial class BalanceViewModel : ObservableObject
	{
		[ObservableProperty]
		private string balance;

		public BalanceViewModel()
		{
			balance = "Loading";
		}

        public async Task GetBalanceAsync()
        {
            Balance = "Loading";
            try
            {
                var client = new Model.AjunaExt.AjunaClientExt(
                    new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")),
                    Ajuna.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

                await client.ConnectAsync();

                var accountInfo = await client.SystemStorage.Account(Model.KeysModel.GetSubstrateKey());

                Balance = accountInfo.Data.Free.Value.ToString();

            }
            catch (Exception ex)
            {
                // this probably means that nothing is saved for that account
                Balance = "0";
            }
        }
    }
}

