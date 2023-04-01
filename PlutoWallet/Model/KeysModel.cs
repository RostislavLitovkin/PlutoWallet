using System;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Schnorrkel.Keys;
using static Substrate.NetApi.Mnemonic;

namespace PlutoWallet.Model
{
	public class KeysModel
	{
		public KeysModel()
		{

		}

        public static string[] GenerateMnemonicsArray()
        {
            var random = new Random();

            var entropyBytes = new byte[24];
            random.NextBytes(entropyBytes);

            var mnemonic = Mnemonic.MnemonicFromEntropy(entropyBytes, BIP39Wordlist.English);

            return mnemonic;
        }

        public static string GetSubstrateKey()
        {
            return GetAccount().Value; // Utils.GetAddressFrom(Utils.HexToByteArray(GetPublicKey()), 42);
        }

        public static string GetPublicKey()
        {
            //return Preferences.Get("publicKey", "Error - no pubKey");
            var array = Utils.GetPublicKeyFrom(GetAccount().Value);
            return "0x" + BitConverter.ToString(array).Replace("-", string.Empty).ToLower(); //str;
            //return Utils.GetAddressFrom(Utils.HexToByteArray(Preferences.Get("publicKey", "Error - no pubKey")), 42);
            
        }

        public static Account GetAccount()
        {
            var miniSecret = new MiniSecret(Utils.HexToByteArray(Preferences.Get("privateKey", "")), ExpandMode.Ed25519);

            /*return Account.Build(
                KeyType.Ed25519,
                Utils.HexToByteArray(Preferences.Get("privateKey", ""), true),
                Utils.HexToByteArray(GetPublicKey(), true));*/

            return Account.Build(KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);
        }
    }
}

