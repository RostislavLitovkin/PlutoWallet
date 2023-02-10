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
        public string PublicKey => KeysModel.GetPublicKey();//.Substring(0, 6) + "..." + KeysModel.GetPublicKey().Substring(62, 4);

        public string SubstrateKey => KeysModel.GetSubstrateKey();

        /*public string ChainKey => Utils.GetAddressFrom(
            Utils.HexToByteArray(KeysModel.GetPublicKey()),
            Metadata.Origin.
        );*/

        [ObservableProperty]
        private string response;

        [ObservableProperty]
        private Metadata metadata;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private string metadataLabel;

        public string Test
        {
            get
            {
                //var methods = new Method();
                return "";
            }
        }

        [RelayCommand]
        private void IncrementCounter()
        {
            //Response = NetworkingModel.RequestSample();
            //Response = KeysModel.GenerateMnemonicsArray();
            
        }

        public MainViewModel()
        {

            //GetMetadataAsync();

            response = "request me ^^";

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

                /*var modules = metadata.NodeMetadata.Modules;

                string moduleKey = "";
                long callKey = 0;

                foreach (string i in modules.Keys)
                {
                    if (modules[i.ToString()].Name == "Balances")
                    {
                        moduleKey = i;
                    }
                }

                string callsTypeId = modules[moduleKey].Calls.TypeId.ToString();
                var calls = metadata.NodeMetadata.Types[callsTypeId];

                foreach (var variant in calls.Variants)
                {
                    if (variant.Name == "transfer")
                    {
                        callKey = variant.Index;
                    }
                }
                //MetadataLabel = "Data: " + modules[moduleKey].Name + " " + metadata.NodeMetadata.Types[callsTypeId].Variants[callKey].Name;

                Console.WriteLine();*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetBalanceAsync()
        {
            Response = "loading";
            try
            {
                var client = new AjunaClientExt(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), ChargeTransactionPayment.Default());

                await client.ConnectAsync();

                var accountInfo = await client.SystemStorage.Account(KeysModel.GetSubstrateKey());

                Response = Utils.Bytes2HexString(KeysModel.GetAccount().Bytes);
                Response = "Balance: " + accountInfo.Data.Free.Value;

            }
            catch (Exception ex)
            {
                MetadataLabel = ex.Message;
            }
        }

        
    }
}
