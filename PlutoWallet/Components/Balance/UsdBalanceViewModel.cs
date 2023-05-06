using System;
using System.Collections.ObjectModel;
using System.Numerics;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.MessagePopup;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.Model.Types.Base;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using PlutoWallet.NetApiExt.Generated.Model.pallet_assets.types;

namespace PlutoWallet.Components.Balance
{
	public partial class UsdBalanceViewModel : ObservableObject
	{
        private const int EXTRA_HEIGHT = 65;

        [ObservableProperty]
        private ObservableCollection<Asset> assets = new ObservableCollection<Asset>();

        [ObservableProperty]
        private double heightRequest;

        [ObservableProperty]
        private string usdSum;

        public UsdBalanceViewModel()
		{
            heightRequest = EXTRA_HEIGHT;
            usdSum = "Loading";
		}

		public async Task GetBalancesAsync()
		{
            double usdSumValue = 0;
            var assetsCollection = new ObservableCollection<Asset>();

            for (int i = 0; i < Model.AjunaClientModel.GroupClients.Length; i++)
            {
                var client = Model.AjunaClientModel.GroupClients[i];
                var endpoint = Model.AjunaClientModel.GroupEndpoints[i];

                if (endpoint.ChainType != "Substrate")
                {
                    assetsCollection.Add(new Asset
                    {
                        Amount = "Unsupported",
                        //Symbol = endpoint.Unit, // I think it looks better without it
                        //ChainIcon = endpoint.Icon,
                        UsdValue = String.Format("{0:0.00}", 0) + " USD",
                    });

                    continue;
                }

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

                // Calculate a real USD value
                double usdValue = amount;

                usdSumValue += usdValue;

                assetsCollection.Add(new Asset
                {
                    Amount = String.Format("{0:0.00}", amount),
                    Symbol = endpoint.Unit,
                    ChainIcon = endpoint.Icon,
                    UsdValue = String.Format("{0:0.00}", usdValue) + " USD",
                });

                /*
                try
                {
                    if (i == 2)
                    {
                        var messagePopup = DependencyService.Get<MessagePopupViewModel>();
                        //var index = BigInteger.Parse("340282366920938463463374607431768211455");

                        var u = new U128();
                        u.Create("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");

                        BaseTuple<U128, AccountId32> tuple = new BaseTuple<U128, AccountId32>();


                        System.Collections.Generic.List<byte> byteArray = new List<byte>();
                        byteArray.AddRange(u.Encode());
                        byteArray.AddRange(Model.KeysModel.GetAccountId32().Encode());

                        tuple.Create(byteArray.ToArray());

                        var assets = await client.AssetsStorage.Asset(u, CancellationToken.None);
                        //var assetMetadata = await client.AssetsStorage.Metadata(u, CancellationToken.None);
                        var values = await client.AssetsStorage.Account(tuple, CancellationToken.None);


                        string key = "0x682a59d51ab9e48a8c8cc418ff9708d2b5f3822e35ca2f31ce3526eab1363fd2e7dafeb873ce4834a974435da87d9cc834050000000000000000000000000000";

                        string result = await client.InvokeAsync<string>(
                            "state_getStorage",
                            new object[] { key, null },
                            CancellationToken.None
                        );

                        AssetMetadata t = new AssetMetadata();
                        t.Create(result);
                        // This is false
                        //messagePopup.Text = values.Balance;


                        //Console.WriteLine(values == null);



                        messagePopup.Title = "Result:";

                        messagePopup.Text = result + "";

                        messagePopup.IsVisible = true;

                        Console.WriteLine("assets:");
                        Console.WriteLine(result);
                    }
                }
                catch (Exception ex)
                {
                    var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                    messagePopup.Title = "AssetsError";
                    messagePopup.Text = ex.Message;

                    messagePopup.IsVisible = true;

                }
                */
            }

            Assets = assetsCollection;

            HeightRequest = (35 * assetsCollection.Count()) + EXTRA_HEIGHT;

            var balanceDashboardViewModel = DependencyService.Get<BalanceDashboardViewModel>();

            balanceDashboardViewModel.RecalculateHeightRequest();

            UsdSum = String.Format("{0:0.00}", usdSumValue) + " USD";
        }
	}
}

