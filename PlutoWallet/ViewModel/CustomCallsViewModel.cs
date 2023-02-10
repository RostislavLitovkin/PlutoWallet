
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Ajuna.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Types;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Ajuna.NetApi.Model.Extrinsics;
using System.Text;

namespace PlutoWallet.ViewModel
{
    public partial class CustomCallsViewModel : ObservableObject
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        [ObservableProperty]
        private Endpoint selectedEndpoint;

        [ObservableProperty]
        private Metadata metadata;

        [ObservableProperty]
        private List<Module> pallets;

        //private List<Variant> calls;
        [ObservableProperty]
        private List<Variant> callsList;

        [ObservableProperty]
        private string args;

        //public bool CallsListEnabled => (CallsList != null && CallsList.Count > 0);

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private string selectedNetworkLabel;
        //public string SelectedNetworkLabel => Preferences.Get("selectedNetworkName", "Polkadot");

        private Module selectedPallet;
        public Module SelectedPallet
        {
            get => selectedPallet;
            set
            {
                SetProperty(ref selectedPallet, value);
                try
                {
                    CallsList = Metadata.NodeMetadata.Types[value.Calls.TypeId.ToString()].Variants.ToList();
                }
                catch
                {
                    CallsList = new List<Variant>(0);
                }
            }
        }

        [ObservableProperty]
        private Variant selectedCall;


        public bool IsSubmitEnabled => true; // (SelectedCall != null && SelectedPallet != null);

        public CustomCallsViewModel()
        {
            loading = true;
        }

        public async Task SubmitCallAsync()
        {
            try
            {

                Console.WriteLine("private key: " + Preferences.Get("privateKey", ""));
                Console.WriteLine("Call method start");
                var primVec = new Ajuna.NetApi.Model.Types.Primitive.Str();
                primVec.Create(Utils.HexToByteArray("0x1862616e616e65"));

                Console.WriteLine((byte)SelectedCall.Index + " " + (byte)SelectedPallet.Index + " " + primVec.Bytes);
                var method = new Method((byte)SelectedPallet.Index, (byte)SelectedCall.Index, primVec.Encode());

                var client = new SubstrateClient(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), ChargeTransactionPayment.Default());
                await client.ConnectAsync();

                Console.WriteLine("Connected");
                await client.Author.SubmitExtrinsicAsync(method, KeysModel.GetAccount(), ChargeTransactionPayment.Default(), 64);

                Console.WriteLine("Success");
                Args = "";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SubmitTransactionAsync()
        {
            var client = new SubstrateClient(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), null);

            //new GenericExtrinsicCall("Balances", "transfer", dest, value);

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

                Pallets = Metadata.NodeMetadata.Modules.Values.ToList();

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
    }
}

