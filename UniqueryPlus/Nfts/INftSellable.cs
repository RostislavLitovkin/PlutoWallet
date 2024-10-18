using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;

namespace UniqueryPlus.Nfts
{
    public interface INftSellable
    {
        public BigInteger? Price { get; set; }
        public Method Sell(BigInteger price);
    }
}
