using Ajuna.NetApi;
using Ajuna.NetApi.Model.Extrinsics;
using Ajuna.NetApi.Model.Meta;
using Ajuna.NetApi.Model.Types;
using Ajuna.NetApi.Model.Types.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace PlutoWallet.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        public string PublicKey => KeysModel.GetPublicKey();

        public string SubstrateKey => KeysModel.GetSubstrateKey();

        [ObservableProperty]
        private string balance;

        [ObservableProperty]
        private Metadata metadata;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private string metadataLabel;

        [ObservableProperty]
        private string dAppName;

        

        [RelayCommand]
        private void IncrementCounter()
        {
            //Response = NetworkingModel.RequestSample();
            //Response = KeysModel.GenerateMnemonicsArray();
            
        }


        // constructor
        public MainViewModel()
        {
            balance = "Balance: loading";
        }

        public async Task GetMetadataAsync()
        {
            Loading = true;
            Console.WriteLine(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io"));
            try
            {
                var client = new SubstrateClient(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), null);
                await client.ConnectAsync();

                Metadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

                Console.WriteLine("Success");

                Loading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetBalanceAsync()
        {
            Balance = "Balance: loading";
            try
            {
                var client = new AjunaClientExt(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), ChargeTransactionPayment.Default());

                await client.ConnectAsync();

                var accountInfo = await client.SystemStorage.Account(KeysModel.GetSubstrateKey());

                Balance = "Balance: " + accountInfo.Data.Free.Value;

            }
            catch (Exception ex)
            {
                Balance = "Balance: 0";
                MetadataLabel = ex.Message;
            }
        }

        
    }
}
