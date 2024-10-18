using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Nfts
{
    public interface INftBuyableWithReceiver
    {
        public bool IsForSale { get; set; }
        public Method Buy(string receiverAddress);
    }
}
