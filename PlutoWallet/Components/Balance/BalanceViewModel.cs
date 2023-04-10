using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Balance
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
                var client = Model.AjunaClientModel.Client;

                var accountInfo = await client.SystemStorage.Account(Model.KeysModel.GetSubstrateKey());

                Balance = accountInfo.Data.Free.Value.ToString();
            }
            catch (Exception ex)
            {
                try
                {
                    var client = Model.AjunaClientModel.Client;
                    // this probably means that nothing is saved for that account
                    Balance = "0";
                }
                catch
                {
                    Balance = "Failed"; ;
                }
            }
        }
    }
}

