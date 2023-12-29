using System;
using PlutoWallet.Model;
using Substrate.NET.Wallet;
using Substrate.NetApi.Model.Types;

namespace PlutoWalletTests
{
	public class Mnemonics
	{
        [Test]
        public async Task GenerateNewAccount()
        {
            Account account = MnemonicsModel.GenerateNewAccount();

            Account account2 = MnemonicsModel.GenerateNewAccount();

            Assert.That(account.Value != account2.Value);
        }

        [Test]
        public async Task GetAccount()
        {
            Account account = MnemonicsModel.GetAccount("flight rent steel toddler casino party exact duck square segment charge swap");

            string addressFromPolkadotJs = "5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ";
            Assert.That(account.Value == addressFromPolkadotJs);
        }

        [Test]
        public async Task ExportJson()
        {
            string json = MnemonicsModel.ExportJson("flight rent steel toddler casino party exact duck square segment charge swap", "PlutoWallet");

            //string expectedJson = "{\"encoded\":\"pVnmo9xOx/G2hZv+RfEefLnprjmvnO5o4LU39rBZJVgAgAAAAQAAAAgAAAAby6SKyPtKfjNxsJ4I8hcsP7+y5gbfQ1+GVHa4/qyHh6n7IGjmqbbtssPIsVARCQ73Ep6xQnCUOPoAt7afQh0qXo7/G44YV7XZV7RDeOcNA9ANN2WWVq6olnn8YqTaIOBY1MSZ2coYjNCAH9Ouajnay63xF+QGyD5/vsVOrHMzSS4o383mPgLdyw6Z+Sx2b0njr0QYTrXBDHZlVY6c\",\"encoding\":{\"content\":[\"pkcs8\",\"sr25519\"],\"type\":[\"scrypt\",\"xsalsa20-poly1305\"],\"version\":\"3\"},\"address\":\"5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ\",\"meta\":{\"genesisHash\":\"\",\"name\":\"PlutoWallet\",\"whenCreated\":1703507035771}}";
            Console.WriteLine(json + "\n\n");
            Assert.That(json.Contains("5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ"));
            Assert.That(json.Contains("ed25519"));

        }

        [Test]
        public async Task ImportAccount()
        {
            string json = "{\"encoded\":\"pVnmo9xOx/G2hZv+RfEefLnprjmvnO5o4LU39rBZJVgAgAAAAQAAAAgAAAAby6SKyPtKfjNxsJ4I8hcsP7+y5gbfQ1+GVHa4/qyHh6n7IGjmqbbtssPIsVARCQ73Ep6xQnCUOPoAt7afQh0qXo7/G44YV7XZV7RDeOcNA9ANN2WWVq6olnn8YqTaIOBY1MSZ2coYjNCAH9Ouajnay63xF+QGyD5/vsVOrHMzSS4o383mPgLdyw6Z+Sx2b0njr0QYTrXBDHZlVY6c\",\"encoding\":{\"content\":[\"pkcs8\",\"sr25519\"],\"type\":[\"scrypt\",\"xsalsa20-poly1305\"],\"version\":\"3\"},\"address\":\"5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ\",\"meta\":{\"genesisHash\":\"\",\"name\":\"PlutoWallet\",\"whenCreated\":1703507035771}}";

            Wallet wallet = MnemonicsModel.ImportJson(json, "PlutoWallet");

            Assert.That( wallet.Account.Value == "5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ");
        }

        [Test]
        public async Task ImportAndExportJson()
        {
            string json = MnemonicsModel.ExportJson("flight rent steel toddler casino party exact duck square segment charge swap", "PlutoWallet");

            //string expectedJson = "{\"encoded\":\"pVnmo9xOx/G2hZv+RfEefLnprjmvnO5o4LU39rBZJVgAgAAAAQAAAAgAAAAby6SKyPtKfjNxsJ4I8hcsP7+y5gbfQ1+GVHa4/qyHh6n7IGjmqbbtssPIsVARCQ73Ep6xQnCUOPoAt7afQh0qXo7/G44YV7XZV7RDeOcNA9ANN2WWVq6olnn8YqTaIOBY1MSZ2coYjNCAH9Ouajnay63xF+QGyD5/vsVOrHMzSS4o383mPgLdyw6Z+Sx2b0njr0QYTrXBDHZlVY6c\",\"encoding\":{\"content\":[\"pkcs8\",\"sr25519\"],\"type\":[\"scrypt\",\"xsalsa20-poly1305\"],\"version\":\"3\"},\"address\":\"5CDYtN4QFWUNtRkoAKB5oSGBCpbMJXEjNDAtQYyqAVmCadYQ\",\"meta\":{\"genesisHash\":\"\",\"name\":\"PlutoWallet\",\"whenCreated\":1703507035771}}";

            Wallet wallet = MnemonicsModel.ImportJson(json, "PlutoWallet");

            Assert.That(true);
        }
    }
}

