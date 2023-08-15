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
    public partial class EnterMnemonicsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string privateKey;

        [ObservableProperty]
        private string mnemonics;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string password2;

        public bool IsStrongPassword => true;//!(Password == null || Password == "");

        public async Task CreateKeys()
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

            Preferences.Set(
                "password",
                Password
            );

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
        }

        public async Task CreateKeysWithPrivateKey()
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

            Preferences.Set(
                "password",
                Password2
            );

            ExpandMode expandMode = ExpandMode.Ed25519;

            var miniSecret = new MiniSecret(Utils.HexToByteArray(PrivateKey), expandMode);

            Account account = Account.Build(
                KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);

            Preferences.Set(
                "privateKey",
                 PrivateKey
            );

            Preferences.Set(
                "publicKey",
                 account.Value
            );

            Preferences.Set("privateKeyExpandMode", 1);

            Preferences.Set("usePrivateKey", true);

            Console.WriteLine("DONE");
        }

        public EnterMnemonicsViewModel()
        {

        }
    }
}
