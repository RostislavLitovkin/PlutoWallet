
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;

namespace PlutoWallet.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        public string PublicKey => KeysModel.GetPublicKey();

        public string SubstrateKey => KeysModel.GetSubstrateKey();
    }
}
