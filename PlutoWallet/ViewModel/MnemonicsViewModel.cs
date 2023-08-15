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
       
        [ObservableProperty]
        private string mnemonics;

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
            Preferences.Set("biometricsEnabled", false);

            try
            {
                // Set biometrics
                for (int i = 0; i < 5; i++)
                {
                    var request = new AuthenticationRequestConfiguration("Biometric verification", "..");

                    var result = await CrossFingerprint.Current.AuthenticateAsync(request);

                    if (result.Authenticated)
                    {
                        // Fingerprint set, perhaps do with it something in the future

                        Preferences.Set("biometricsEnabled", true);

                        break;
                    }
                    else
                    {

                    }
                }
            }
            catch
            {

            }

            // This is default, could be changed in the future or with a setting
            ExpandMode expandMode = ExpandMode.Ed25519;

            var secret = Mnemonic.GetSecretKeyFromMnemonic(Mnemonics, Password, BIP39Wordlist.English);

            var miniSecret = new MiniSecret(secret, expandMode);

            Account account = Account.Build(
                KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);

            Preferences.Set(
                "publicKey",
                 account.Value
            );

            Preferences.Set(
                "mnemonics",
                 Mnemonics
            );

            Preferences.Set("privateKeyExpandMode", 1);

            Preferences.Set("usePrivateKey", false);

            return true;
        }

        public MnemonicsViewModel()
        {
            var mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
            string temp = string.Empty;

            foreach (string mnemonic in mnemonicsArray)
            {
                temp += " " + mnemonic;
            }

            Mnemonics = temp.Trim();
        }
    }
}

