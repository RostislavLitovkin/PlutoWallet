using System;
using Ajuna.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static Ajuna.NetApi.Mnemonic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Schnorrkel.Keys;

namespace PlutoWallet.ViewModel
{
    public partial class EnterMnemonicsViewModel : ObservableObject
    {


        [ObservableProperty]
        private string[] mnemonicsArray = new string[18];

        [ObservableProperty]
        private string password;

        public bool IsStrongPassword => true;//!(Password == null || Password == "");

        public void CreateKeys()
        {
            var mnemonicsString = string.Empty;
            foreach (var item in mnemonicsArray)
            {
                mnemonicsString += item + " ";
            }

            var keyPair = Mnemonic.GetKeyPairFromMnemonic(mnemonicsString.Trim(), Password, BIP39Wordlist.English, ExpandMode.Ed25519);
            var secret = Mnemonic.GetSecretKeyFromMnemonic(mnemonicsString.Trim(), Password, BIP39Wordlist.English);

            Preferences.Set(
                "privateKey",
                Utils.Bytes2HexString(keyPair.Secret.key.GetBytes())
            );
            Preferences.Set(
                "publicKey",
                 Utils.Bytes2HexString(keyPair.Public.Key)
            );
        }

        public EnterMnemonicsViewModel()
        {

        }
    }
}
