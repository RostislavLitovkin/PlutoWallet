using System;
using Substrate.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static Substrate.NetApi.Mnemonic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using Schnorrkel.Keys;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Substrate.NetApi.Model.Types;

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
        private string[] orderedMnemonicsArray;

        public async Task<bool> Continue()
        {
            var request = new AuthenticationRequestConfiguration("Biometric verification", "..");

            var result = await CrossFingerprint.Current.AuthenticateAsync(request);

            if (result.Authenticated)
            {
                // Fingerprint set, perhaps do with it something in the future

                Preferences.Set(
                    "password",
                     Password
                );
            }
            else
            {
                return false;
            }


            var mnemonicsString = string.Empty;
            foreach (var item in MnemonicsArray)
            {
                mnemonicsString += item + " ";
            }

            // This is default, could be changed in the future or with a setting
            ExpandMode expandMode = ExpandMode.Uniform;

            var keyPair = Mnemonic.GetKeyPairFromMnemonic(mnemonicsString.Trim(), Password, BIP39Wordlist.English, expandMode);

            var miniSecret = new MiniSecret(keyPair.Secret.key.GetBytes(), expandMode);

            Account account = Account.Build(KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);

            Preferences.Set(
                "publicKey",
                 account.Value
            );

            Preferences.Set(
                "mnemonics",
                 mnemonicsString.Trim()
            );

            Preferences.Set("privateKeyExpandMode", 0);

            return true;
        }

        public MnemonicsViewModel()
        {
            mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
            orderedMnemonicsArray = new string[mnemonicsArray.Count()];

            int i = 0;
            foreach (string mnemonic in mnemonicsArray)
            {
                orderedMnemonicsArray[i] = ++i + ". " + mnemonic;
            }
        }
    }
}

