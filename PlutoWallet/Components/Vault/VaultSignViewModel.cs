using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Components.Vault
{
    public partial class VaultSignViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string palletIndex;

        [ObservableProperty]
        private string callIndex;

        [ObservableProperty]
        private string parameters;

        [ObservableProperty]
        private Method ajunaMethod;

        public byte[] Payload { get; set; }

        [ObservableProperty]
        private string signature;

        [ObservableProperty]
        private bool signatureIsVisible = !false;

        [ObservableProperty]
        private bool signButtonIsVisible = true;

        public VaultSignViewModel()
        {
            signature = "Loading";
        }

        public async Task SignExtrinsicAsync(string encodedBytes)
        {
            var clientExt = await Model.AjunaClientModel.GetMainClientAsync();
            var client = clientExt.SubstrateClient;

            if (!client.IsConnected)
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Not connected";
                messagePopup.Text = "You need to connect to the chain. Check your Internet connection.";

                messagePopup.IsVisible = true;

                return;
            }

            SignatureIsVisible = !false;

            SignButtonIsVisible = true;

            string address = Utils.GetAddressFrom(Utils.HexToByteArray(encodedBytes.Substring(0, 66)));

            int offset = 0;

            int callLength = CompactInteger.Decode(Utils.HexToByteArray(encodedBytes.Substring(66, 8)), ref offset);

            string methodBytes = encodedBytes.Substring(66 + offset * 2, callLength * 2);

            Method method = new Method(Utils.HexToByteArray(methodBytes.Substring(0, 2))[0], Utils.HexToByteArray(methodBytes.Substring(2, 2))[0], Utils.HexToByteArray(methodBytes.Substring(4)));

            try
            {
                var pallet = client.MetaData.NodeMetadata.Modules[method.ModuleIndex];

                PalletIndex = pallet.Name;
                CallIndex = clientExt.CustomMetadata.NodeMetadata.Types[pallet.Calls.TypeId.ToString()]
                    .Variants[method.CallIndex].Name;
            }
            catch
            {
                PalletIndex = "(" + method.ModuleIndex.ToString() + " index)";
                CallIndex = "(" + method.CallIndex.ToString() + " index)";
            }

            Parameters = Model.PalletCallModel.GetJsonMethod(clientExt, method);

            AjunaMethod = method;

            string extensionBytes = encodedBytes.Substring(66 + offset * 2 + callLength * 2);

            string genesisHash = client.GenesisHash.Value;

            extensionBytes = extensionBytes.Substring(0, extensionBytes.LastIndexOf(genesisHash.Substring(2)));

            byte[] payload = Utils.HexToByteArray(methodBytes + extensionBytes);

            if (payload.Length > 256) payload = HashExtension.Blake2(payload, 256);

            Payload = payload;

            IsVisible = true;

            return;
        }
    }
}

