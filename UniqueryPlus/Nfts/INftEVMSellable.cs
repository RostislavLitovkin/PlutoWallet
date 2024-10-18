using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;

namespace UniqueryPlus.Nfts
{
    public interface INftEVMSellable
    {
        public BigInteger? Price { get; set; }
        public Method Sell(BigInteger price, string sender);
    }
}
