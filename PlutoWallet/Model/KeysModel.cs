using System;
using Ajuna.NetApi;
using static Ajuna.NetApi.Mnemonic;

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
    }
}

