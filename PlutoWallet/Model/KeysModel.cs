using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using PlutoWallet.Components.ConfirmTransaction;
using Substrate.NET.Wallet;
using Substrate.NET.Schnorrkel.Keys;

namespace PlutoWallet.Model
{
    public class KeysModel
    {

        public static async Task GenerateNewAccountAsync(string password)
        {
            Console.WriteLine("Generate started");

            string mnemonics = MnemonicsModel.GenerateMnemonics();

            Console.WriteLine("New mnemonics generated");

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
            // ExpandMode expandMode = ExpandMode.Ed25519;

            Account account = MnemonicsModel.GetAccount(mnemonics);

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
            // This is default, could be changed in the future or with a setting
            ExpandMode expandMode = ExpandMode.Ed25519;

            var miniSecret = new MiniSecret(Utils.HexToByteArray(privateKey), expandMode);

            Account account = Account.Build(
                KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToEd25519Bytes(),
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

        public static async Task GenerateNewAccountFromJsonAsync(string json)
        {
            Wallet wallet = MnemonicsModel.ImportJson(json, await SecureStorage.Default.GetAsync("password"));

            if (wallet.IsLocked)
            {
                throw new Exception("Bad password");
            }

            Account account = wallet.Account;

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

                // use Private key
                if (Preferences.Get("usePrivateKey", false))
                {
                    var miniSecret2 = new MiniSecret(Utils.HexToByteArray(mnemonicsOrPrivateKey), expandMode);

                    return Option<Account>.Some(Account.Build(KeyType.Sr25519,
                        miniSecret2.ExpandToSecret().ToEd25519Bytes(),
                        miniSecret2.GetPair().Public.Key));
                }

                return Option<Account>.Some(MnemonicsModel.GetAccount(mnemonicsOrPrivateKey));
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

