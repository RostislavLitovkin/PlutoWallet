
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Substrate.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Types;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using System.Text;
using Substrate.NetApi.Model.Types.Metadata.V14;
using Substrate.NetApi.Model.Meta;
using PlutoWallet.Components.ArgumentsView;
using System.Collections.ObjectModel;

namespace PlutoWallet.ViewModel
{
    public partial class CustomCallsViewModel : ObservableObject
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        [ObservableProperty]
        private string argName;

        [ObservableProperty]
        private string argType;

        [ObservableProperty]
        private string argTypeIndex;

        [ObservableProperty]
        private ArgEntryType arg2;

        [ObservableProperty]
        private ArgEntryType arg3;

        [ObservableProperty]
        private Endpoint selectedEndpoint;

        [ObservableProperty]
        private Metadata customMetadata;

        [ObservableProperty]
        private NodeMetadataV14 metadata;

        [ObservableProperty]
        private List<PalletModule> pallets;

        //private List<Variant> calls;
        [ObservableProperty]
        private List<Types.Variant> callsList;

        [ObservableProperty]
        private string args;

        //public bool CallsListEnabled => (CallsList != null && CallsList.Count > 0);

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private string selectedNetworkLabel;
        //public string SelectedNetworkLabel => Preferences.Get("selectedNetworkName", "Polkadot");

        private PalletModule selectedPallet;
        public PalletModule SelectedPallet
        {
            get => selectedPallet;
            set
            {
                SetProperty(ref selectedPallet, value);
                try
                {
                    if (Metadata.Types[value.Calls.TypeId].TypeDef == TypeDefEnum.Variant)
                    {
                        CallsList = CustomMetadata.NodeMetadata.Types[value.Calls.TypeId.ToString()].Variants.ToList();
                        var nothing = CustomMetadata.NodeMetadata.Types[value.Calls.TypeId.ToString()].TypeParams;
                    }
                }
                catch
                {
                    CallsList = new List<Types.Variant>(0);
                }
            }
        }

        
        private Types.Variant selectedCall;

        public Types.Variant SelectedCall
        {
            get => selectedCall;
            set
            {
                selectedCall = value;
                ErrorMessage = "";

                if (value == null)
                {
                    return;
                }

                var argumentsViewModel = DependencyService.Get<ArgumentsViewModel>();

                argumentsViewModel.Args = new ObservableCollection<ArgProperties>();

                try
                {

                    if (value.TypeFields.Length > 5)
                    {
                        throw new Exception("The call has got too many arguments. It is currently unsupported.");
                    }

                    for (int i = 0; i < value.TypeFields.Length; i++)
                    {
                        var argProperties = new ArgProperties();
                        var typeField = value.TypeFields[i];
                        
                        argProperties.Name = typeField.Name;

                        if (value.TypeFields[1].TypeId != null)
                        {
                            RecursiveDetermineArg(
                                argProperties,
                                i,
                                CustomMetadata.NodeMetadata.Types[typeField.TypeId.ToString()]);
                        }
                        else
                        {
                            argProperties.Type = typeField.TypeName;
                        }

                        argumentsViewModel.Args.Add(argProperties);
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage = ex.Message;
                }

            }
        }


        private void RecursiveDetermineArg(ArgProperties argProperties, int index, TypeValue type)
        {

            if (type.TypeDef == TypeDef.Primitive)
            {
                argProperties.Type = type.Primitive;
            }
            else if (type.TypeDef == TypeDef.Variant)
            {
                // Add the option to select the different variants

                // Add the option to enter multiple type fields (So probably a foreach)

                RecursiveDetermineArg(
                            argProperties,
                            index,
                            CustomMetadata.NodeMetadata.Types[type.Variants[0].TypeFields[0].TypeId.ToString()]);
            }
            else if (type.TypeDef == TypeDef.Compact)
            {
                RecursiveDetermineArg(
                            argProperties,
                            index,
                            CustomMetadata.NodeMetadata.Types[type.TypeId.ToString()]);
            }
            else if (type.TypeDef == TypeDef.Composite)
            {
                // Add the option to enter multiple type fields (So probably a foreach)

                argProperties.Type = type.TypeFields[0].TypeName;
            }
            else
            {
                throw new Exception("The call is currently unsupported.");
            }

        }

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
                var primVec = new Substrate.NetApi.Model.Types.Primitive.Str();
                primVec.Create(Utils.HexToByteArray("0x1862616e616e65"));

                Console.WriteLine((byte)SelectedCall.Index + " " + (byte)SelectedPallet.Index + " " + primVec.Bytes);
                var method = new Method((byte)SelectedPallet.Index, (byte)SelectedCall.Index, primVec.Encode());

                var client = new SubstrateClient(new Uri(Preferences.Get("selectedNetwork", "wss://rpc.polkadot.io")), ChargeTransactionPayment.Default());
                await client.ConnectAsync();

                Console.WriteLine("Connected");

                if ((await KeysModel.GetAccount()).IsSome(out var account)) {
                    await client.Author.SubmitExtrinsicAsync(method, account, ChargeTransactionPayment.Default(), 64);
                }

                Console.WriteLine("Success");
                Args = "";


            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessage = ex.Message;
            }
        }

        public async Task SubmitTransactionAsync()
        {
            var client = Model.AjunaClientModel.Client;

            //..
        }

        public void GetMetadata()
        {
            ErrorMessage = "";

            Loading = true;

            try
            {
                var client = Model.AjunaClientModel.Client;

                Metadata = client.MetaData.NodeMetadata;
                CustomMetadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

                Pallets = client.MetaData.NodeMetadata.Modules.Values.ToList<PalletModule>();

                Loading = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessage = ex.Message;
            }
        }
    }

    public class ArgEntryType
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
    }
}

