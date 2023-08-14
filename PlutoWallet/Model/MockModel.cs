using System;
using Schnorrkel.Keys;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi;

namespace PlutoWallet.Model
{
    public class MockModel
    {
        public static Account GetMockAccount()
        {
            var miniSecret = new MiniSecret(Utils.HexToByteArray("0x34847CDDAA7E3E253D6DC7B7BB1D819B6469B8256C28F46E8CC6D07DA91B240AD3B76034856F5DCB0DF96194ABFD0AF1E1202554C99565E5C65F86CB69BC6D9D"), ExpandMode.Ed25519);

            return Account.Build(KeyType.Sr25519,
                miniSecret.ExpandToSecret().ToBytes(),
                miniSecret.GetPair().Public.Key);
        }
    }
}

