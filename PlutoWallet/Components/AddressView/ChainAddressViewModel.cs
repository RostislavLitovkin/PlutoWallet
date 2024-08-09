using CommunityToolkit.Mvvm.ComponentModel;
using Substrate.NetApi;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Components.AddressView
{
    public partial class ChainAddressViewModel : ObservableObject
    {
        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private string qrAddress;

        [ObservableProperty]
        private string chainAddressName;

        [ObservableProperty]
        private bool isVisible;

        public ChainAddressViewModel()
        {
            qrAddress = "Loading";
            address = KeysModel.GetPublicKey();
            chainAddressName = "";
            isVisible = true;
        }

        public void SetChainAddress(SubstrateClientExt client)
        {
            var endpoint = client.Endpoint;

            if (endpoint.Name == "(Local)ws://127.0.0.1:9944")
            {
                IsVisible = false;
                return;
            }

            if (endpoint.Name.Length <= 10)
            {
                ChainAddressName = endpoint.Name + " key";
            }
            else
            {
                ChainAddressName = endpoint.Name.Split(" ")[0] + " key";
            }

            Address = Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), endpoint.SS58Prefix);

            try
            {
                QrAddress = "substrate:" + Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), endpoint.SS58Prefix) + ":" + client.SubstrateClient.GenesisHash;

            } catch {
                QrAddress = "substrate:" + Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), endpoint.SS58Prefix);
            }

            IsVisible = true;
        }
    }
}

