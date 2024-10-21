using Substrate.NetApi.Model.Extrinsics;
using System.Numerics;

namespace UniqueryPlus.Nfts
{
    public interface INftBuyable
    {
        public BigInteger? Price { get; set; }
        public bool IsForSale { get; set; }
        public Method Buy();
    }
}
