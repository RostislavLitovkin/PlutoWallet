using System;
using Substrate.NetApi.Model.Extrinsics;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using PlutoWallet.Types;

namespace PlutoWallet.Components.TransactionRequest
{
    public partial class TransactionRequestViewModel : ObservableObject
    {
        [ObservableProperty]
        private string palletIndex;

        [ObservableProperty]
        private string callIndex;

        [ObservableProperty]
        private string parameters;

        private Method ajunaMethod;

        public Method AjunaMethod
        {
            get => ajunaMethod;
            set
            {
                ajunaMethod = value;
                try
                {
                    var client = Model.AjunaClientModel.Client;

                    var pallet = client.MetaData.NodeMetadata.Modules[value.ModuleIndex];
                    Metadata metadata = JsonConvert.DeserializeObject<Metadata>(client.MetaData.Serialize());

                    PalletIndex = pallet.Name;
                    CallIndex = metadata.NodeMetadata.Types[pallet.Calls.TypeId.ToString()]
                        .Variants[value.CallIndex].Name;
                }
                catch
                {
                    PalletIndex = "(" + value.ModuleIndex.ToString() + " index)"; 
                    CallIndex = "(" + value.CallIndex.ToString() + "index)";
                }

                if (value.Parameters.Length > 5)
                {
                    Parameters = "0x" + Convert.ToHexString(value.Parameters).Substring(0, 10) + "..";
                }
                else
                {
                    Parameters = "0x" + Convert.ToHexString(value.Parameters);
                }
            }
        }

        [ObservableProperty]
        private bool isVisible;

        public TransactionRequestViewModel()
        {
            isVisible = false;
        }
    }
}

