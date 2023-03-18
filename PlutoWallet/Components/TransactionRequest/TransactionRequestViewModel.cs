using System;
using Ajuna.NetApi.Model.Extrinsics;
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

                    PalletIndex = "Pallet: " + pallet.Name;
                    CallIndex = "Call: " + metadata.NodeMetadata.Types[pallet.Calls.TypeId.ToString()]
                        .Variants[value.CallIndex].Name;
                }
                catch
                {
                    PalletIndex = "Pallet index: " + value.ModuleIndex;
                    CallIndex = "Call index: " + value.CallIndex;
                }

                if (value.Parameters.Length > 5)
                {
                    Parameters = "Parameters: 0x" + Convert.ToHexString(value.Parameters).Substring(0, 10) + "..";
                }
                else
                {
                    Parameters = "Parameters: 0x" + Convert.ToHexString(value.Parameters);
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

