using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Nfts
{
    public interface INftBuyable
    {
        public bool IsForSale { get; set; }
        public Method Buy();
    }
}
