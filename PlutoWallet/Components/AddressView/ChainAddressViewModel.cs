using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using Substrate.NetApi;
using PlutoWallet.Model;

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

        public void SetChainAddress()
        {
            var endpoint = AjunaClientModel.SelectedEndpoint;

            var client = AjunaClientModel.Client;

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
            QrAddress = "substrate:" + Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), endpoint.SS58Prefix) + ":" + client.GenesisHash;
            IsVisible = true;
        }
    }
}

