using System;
using Substrate.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static Substrate.NetApi.Mnemonic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Schnorrkel.Keys;

namespace PlutoWallet.ViewModel
{
    public partial class MnemonicsViewModel : ObservableObject //, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        [ObservableProperty]
        private string[] mnemonicsArray;

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password == value)
                    return;

                password = value;

                // tell the user that his password is weak

                //RaisePropertyChanged(nameof(Password));
                //RaisePropertyChanged(nameof(IsStrongPassword));
            }
        }

        public bool IsStrongPassword => true;//!(Password == null || Password == "");

        [ObservableProperty]
        private string debugText;

        [ObservableProperty]
        private string[] orderedMnemonicsArray;

        public void Continue()
        {
            var mnemonicsString = string.Empty;
            foreach (var item in MnemonicsArray)
            {
                mnemonicsString += item + " ";
            }

            var keyPair = Mnemonic.GetKeyPairFromMnemonic(mnemonicsString.Trim(), Password, BIP39Wordlist.English, ExpandMode.Ed25519);
            var secret = Mnemonic.GetSecretKeyFromMnemonic(mnemonicsString.Trim(), Password, BIP39Wordlist.English);

            DebugText = Utils.Bytes2HexString(keyPair.Secret.key.GetBytes()).Substring(0, 8) +
                "\n" + Utils.Bytes2HexString(secret).Substring(0, 8) +
                "\n" + Utils.Bytes2HexString(new MiniSecret(secret, ExpandMode.Ed25519).GetPair().Secret.key.GetBytes()).Substring(0, 8);
            
            Console.WriteLine("I am here");

            Preferences.Set(
                "privateKey",
                Utils.Bytes2HexString(keyPair.Secret.key.GetBytes())
            );
            Preferences.Set(
                "publicKey",
                 Utils.Bytes2HexString(keyPair.Public.Key)
            );
        }

        public MnemonicsViewModel()
        {
            mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
            orderedMnemonicsArray = new string[mnemonicsArray.Count()];

            debugText = "Hello";

            int i = 0;
            foreach (string mnemonic in mnemonicsArray)
            {
                orderedMnemonicsArray[i] = ++i + ". " + mnemonic;
            }
        }

        /*private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
    }
}

