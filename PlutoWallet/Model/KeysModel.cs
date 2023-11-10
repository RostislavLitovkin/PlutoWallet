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

        public static async Task GenerateNewAccountAsync(string password)
        {
            var mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
            string mnemonics = string.Empty;

            foreach (string mnemonic in mnemonicsArray)
            {
                mnemonics += " " + mnemonic;
            }

            mnemonics = mnemonics.Trim();

            await GenerateNewAccountAsync(mnemonics, password);
        }

        public static async Task GenerateNewAccountAsync(string mnemonics, string password)
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

            var secret = Mnemonic.GetSecretKeyFromMnemonic(mnemonics, password, BIP39Wordlist.English);

            var miniSecret = new MiniSecret(secret, expandMode);

            Account account = Account.Build(
                KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);

            Preferences.Set(
                "publicKey",
                 account.Value
            );

            await SecureStorage.Default.SetAsync(
                "mnemonics",
                 mnemonics
            );

            await SecureStorage.Default.SetAsync(
                "password",
                password
            );

            Preferences.Set("privateKeyExpandMode", 1);

            Preferences.Set("usePrivateKey", false);
        }

        public static async Task GenerateNewAccountFromPrivateKeyAsync(string privateKey)
        {
            ExpandMode expandMode = ExpandMode.Ed25519;

            var miniSecret = new MiniSecret(Utils.HexToByteArray(privateKey), expandMode);

            Account account = Account.Build(
                KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);

            await SecureStorage.Default.SetAsync(
                "privateKey",
                 privateKey
            );

            Preferences.Set(
                "publicKey",
                 account.Value
            );

            Preferences.Set("privateKeyExpandMode", 1);

            Preferences.Set("usePrivateKey", true);
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
            return Utils.GetPublicKeyFrom(GetSubstrateKey());
        }

        /// <summary>
        /// Call this method when you want get the mnemonics or private key
        ///
        /// To use correctly, use this line:
        ///
        /// if ((await KeysModel.GetMnemonicsOrPrivateKeyAsync()).IsSome(out (string, bool) secretValues))
        /// {
        ///     var(mnemonicsOrPrivateKey, usePrivateKey) = secretValues;
        ///     ...
        /// }
        /// </summary>
        public static async Task<Option<(string, bool)>> GetMnemonicsOrPrivateKeyAsync()
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
                if (Preferences.Get("usePrivateKey", false))
                {
                    return Option<(string, bool)>.Some((await SecureStorage.Default.GetAsync("privateKey"), true));
                }
                else
                {
                    return Option<(string, bool)>.Some((await SecureStorage.Default.GetAsync("mnemonics"), false));
                }
            }
            else // Request password instead..
            {
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
                    if (Preferences.Get("usePrivateKey", false))
                    {
                        return Option<(string, bool)>.Some((await SecureStorage.Default.GetAsync("privateKey"), true));
                    }
                    else
                    {
                        return Option<(string, bool)>.Some((await SecureStorage.Default.GetAsync("mnemonics"), false));
                    }
                }
                else if (viewModel.Status == ConfirmTransactionStatus.Denied)
                {
                    return Option<(string, bool)>.None;
                }
            }

            return Option<(string, bool)>.None;
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

            if ((await KeysModel.GetMnemonicsOrPrivateKeyAsync()).IsSome(out (string, bool) secretValues))
            {
                var (mnemonicsOrPrivateKey, _usePrivateKey) = secretValues;

                if (Preferences.Get("usePrivateKey", false))
                {
                    var miniSecret2 = new MiniSecret(Utils.HexToByteArray(mnemonicsOrPrivateKey), expandMode);

                    return Option<Account>.Some(Account.Build(KeyType.Sr25519,
                        miniSecret2.ExpandToSecret().ToBytes(),
                        miniSecret2.GetPair().Public.Key));
                }

                var secret = Mnemonic.GetSecretKeyFromMnemonic(mnemonicsOrPrivateKey, await SecureStorage.Default.GetAsync("password"), BIP39Wordlist.English);

                var miniSecret = new MiniSecret(secret, expandMode);

                return Option<Account>.Some(Account.Build(KeyType.Sr25519,
                    miniSecret.ExpandToSecret().ToBytes(),
                    miniSecret.GetPair().Public.Key));
            }

            return Option<Account>.None;
        }

        public static AccountId32 GetAccountId32()
        {
            var accountId = new AccountId32();
            accountId.Create(GetPublicKeyBytes());

            return accountId;
        }
    }
}

