using Substrate.NetApi.Model.Types;

namespace PlutoWallet.Model
{
    public class PlutoAccountBase : Account
    {

        /// <summary>
        /// https://polkadot.js.org/docs/api/FAQ/#i-cannot-send-transactions-sending-yields-decoding-failures
        /// </summary>
        public uint AddressVersion = 2u;
        public override byte[] Encode()
        {
            List<byte> list = new List<byte>();
            switch (AddressVersion)
            {
                case 0u:
                    return base.Bytes;
                case 1u:
                    list.Add(byte.MaxValue);
                    list.AddRange(base.Bytes);
                    return list.ToArray();
                case 2u:
                    list.Add(0);
                    list.AddRange(base.Bytes);
                    return list.ToArray();
                default:
                    throw new NotImplementedException("Unknown address version please refer to PlutoAccountBase");
            }
        }
    }
}
