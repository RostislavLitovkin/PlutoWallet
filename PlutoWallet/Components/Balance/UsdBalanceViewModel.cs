using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Balance
{
	public partial class UsdBalanceViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<Asset> assets = new ObservableCollection<Asset>();

        public UsdBalanceViewModel()
		{

		}

		public async Task GetBalancesAsync()
		{
            var assetsCollection = new ObservableCollection<Asset>();

            for (int i = 0; i < Model.AjunaClientModel.GroupClients.Length; i++)
            {
                var client = Model.AjunaClientModel.GroupClients[i];
                var endpoint = Model.AjunaClientModel.GroupEndpoints[i];
                double amount = 0;

                try
                {
                    var accountInfo = await client.SystemStorage.Account(Model.KeysModel.GetSubstrateKey());

                    amount = (double)accountInfo.Data.Free.Value / Math.Pow(10, endpoint.Decimals);
                }
                catch
                {
                    // this usually means that nothing is saved for this account
                    
                }

                assetsCollection.Add(new Asset
                {
                    Amount = String.Format("{0:0.00}", amount),
                    Symbol = endpoint.Unit,
                    ChainIcon = endpoint.Icon,
                    UsdValue = String.Format("{0:0.00}", amount) + " USD",
                });
            }

            Assets = assetsCollection;
        }
	}
}

