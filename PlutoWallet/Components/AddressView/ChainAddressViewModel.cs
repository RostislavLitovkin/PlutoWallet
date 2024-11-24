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
            address = "Loading";
            chainAddressName = "";
            isVisible = true;
        }

        public void SetChainAddress(SubstrateClientExt client)
        {
            var endpoint = client.Endpoint;

            if (endpoint.Name.Length <= 10)
            {
                ChainAddressName = endpoint.Name + " key";
            }
            else
            {
                ChainAddressName = endpoint.Name.Split(" ")[0] + " key";
            }

            if (endpoint.ChainType == Constants.ChainType.Substrate)
            {
                Address = Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), endpoint.SS58Prefix);

                try
                {
                    QrAddress = "substrate:" + Address + ":" + client.SubstrateClient.GenesisHash;
                }
                catch
                {
                    QrAddress = "substrate:" + Address;
                }
            }
            else if (endpoint.ChainType == Constants.ChainType.Ethereum)
            {
                Address = Utils.Bytes2HexString(KeysModel.GetPublicKeyBytes()).Substring(0,42);

                QrAddress = Address;
            }



            IsVisible = true;
        }
    }
}

