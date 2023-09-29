using System;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Schnorrkel.Keys;
using static Substrate.NetApi.Mnemonic;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using PlutoWallet.Components.ConfirmTransaction;
using System.Security.Cryptography;

namespace PlutoWallet.Model
{
    public class KeysModel
    {
        public KeysModel()
        {

        }

        public static string[] GenerateMnemonicsArray()
        {
            var random = RandomNumberGenerator.Create();

            var entropyBytes = new byte[16];
            random.GetBytes(entropyBytes);

            var mnemonic = Mnemonic.MnemonicFromEntropy(entropyBytes, BIP39Wordlist.English);

            return mnemonic;
        }

        public static string GetSubstrateKey()
        {
            return Preferences.Get("publicKey", "Error - no pubKey");
        }

        public static string GetPublicKey()
        {
            // publicKey should be always saved
            var array = GetPublicKeyBytes();
            return Utils.Bytes2HexString(array);
        }

        public static byte[] GetPublicKeyBytes()
        {
            // publicKey should be always saved
            return Utils.GetPublicKeyFrom(Preferences.Get("publicKey", "Error - no pubKey"));
        }

        /// <summary>
        /// Call this method when you want to sign a message or transaction.
        ///
        /// To use correctly, use this line:
        ///
        /// if ((await KeysModel.GetAccount()).IsSome(out var account))
        /// </summary>
        public static async Task<Option<Account>> GetAccount()
        {
            var biometricsEnabled = Preferences.Get("biometricsEnabled", false);

            var request = new AuthenticationRequestConfiguration("Biometric verification", "..");
            FingerprintAuthenticationResult result;

            if (biometricsEnabled)
            {
                result = await CrossFingerprint.Current.AuthenticateAsync(request);
            }
            else
            {
                result = new FingerprintAuthenticationResult
                {
                    Status = FingerprintAuthenticationResultStatus.Denied,
                };
            }


            if (result.Authenticated)
            {
                // Fingerprint set, perhaps do with it something in the future
            }
            else
            {
                // Request password instead..
                Console.WriteLine("Authentication failed");

                var viewModel = DependencyService.Get<ConfirmTransactionViewModel>();

                viewModel.Status = ConfirmTransactionStatus.Waiting;
                viewModel.Password = "";
                viewModel.ErrorIsVisible = false;
                viewModel.IsVisible = true;


                while (viewModel.Status == ConfirmTransactionStatus.Waiting)
                {
                    await Task.Delay(500);
                }

                if (viewModel.Status == ConfirmTransactionStatus.Verified)
                {
                    // Do verified animation

                }
                else if (viewModel.Status == ConfirmTransactionStatus.Denied)
                {
                    return Option<Account>.None;
                }
            }

            ExpandMode expandMode;

            switch (Preferences.Get("privateKeyExpandMode", 1))
            {
                case 0:
                    expandMode = ExpandMode.Uniform;
                    break;
                case 1:
                    expandMode = ExpandMode.Ed25519;
                    break;
                default:
                    expandMode = ExpandMode.Uniform;
                    break;
            }

            if (Preferences.Get("usePrivateKey", false))
            {
                var miniSecret2 = new MiniSecret(Utils.HexToByteArray(Preferences.Get("privateKey", "")), expandMode);

                return Option<Account>.Some(Account.Build(KeyType.Sr25519,
                    miniSecret2.ExpandToSecret().ToBytes(),
                    miniSecret2.GetPair().Public.Key));
            }

            var secret = Mnemonic.GetSecretKeyFromMnemonic(Preferences.Get("mnemonics", ""), Preferences.Get("password", ""), BIP39Wordlist.English);

            var miniSecret = new MiniSecret(secret, expandMode);

            return Option<Account>.Some(Account.Build(KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key));
        }

        public static AccountId32 GetAccountId32()
        {
            var accountId = new AccountId32();
            accountId.Create(GetPublicKeyBytes());

            return accountId;
        }
    }
}

