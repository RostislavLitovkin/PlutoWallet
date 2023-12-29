using System;
using System.Security.Cryptography;
using Substrate.NET.Wallet;
using Substrate.NET.Wallet.Keyring;
using Substrate.NetApi;
using Substrate.NetApi.Extensions;
using Substrate.NetApi.Model.Types;
using static Substrate.NetApi.Mnemonic;

namespace PlutoWallet.Model
{
	public class MnemonicsModel
	{
        private static readonly Meta META = new Meta() { name = "PlutoWallet" };

        public static string[] GenerateMnemonicsArray()
        {
            return MnemonicFromEntropy(new byte[16].Populate(), BIP39Wordlist.English); ;
        }

        public static string GenerateMnemonics()
        {
            var mnemonicsArray = GenerateMnemonicsArray();
            string mnemonics = string.Empty;

            foreach (string mnemonic in mnemonicsArray)
            {
                mnemonics += " " + mnemonic;
            }

            return mnemonics.Trim();
        }

        public static Account GenerateNewAccount()
        {
            var mnemonics = GenerateMnemonics();

            return GetAccount(mnemonics);
        }

        public static (Account, string) GenerateNewAccountAndMnemonics()
        {
            var mnemonics = GenerateMnemonics();

            return (GetAccount(mnemonics), mnemonics);
        }

        public static Account GetAccount(string mnemonics)
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            Wallet wallet = keyring.AddFromMnemonic(mnemonics, META, Substrate.NetApi.Model.Types.KeyType.Sr25519);

            return wallet.Account;
        }

        public static string ExportJson(string mnemonics, string password)
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            Wallet wallet = keyring.AddFromMnemonic(mnemonics, META, Substrate.NetApi.Model.Types.KeyType.Sr25519);

            return wallet.ToJson("PlutoWallet", password);
        }

        /// <summary>
        /// Imports the Json string and returns Wallet
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Wallet ImportJson(string json, string password)
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            // Later remove .Replace(..)
            Wallet wallet = keyring.AddFromJson(json.Replace("\"3\"", "3"));
            wallet.Unlock(password);

            return wallet;
        }

        public static Wallet GetWalletFromPair(PairInfo pair, string password)
        {
            var setup = new KeyringAddress(KeyType.Sr25519);

            short ss58Format = 42;

            return new Wallet(
                setup.ToSS58(pair.PublicKey, ss58Format),
                null,
                META,
                pair.PublicKey,
                pair.SecretKey,
                KeyType.Sr25519,
                null
            );
        }

        public static string ExportJsonFromPair(PairInfo pair, string password)
        {
            Wallet wallet = GetWalletFromPair(pair, password);

            Account account = wallet.Account;

            return wallet.ToJson("PlutoWallet", password);
        }
    }
}

