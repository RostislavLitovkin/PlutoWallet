using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Nfts
{
    public interface INftEVMBuyableWithReceiver
    {
        public bool IsForSale { get; set; }
        public Method Buy(string receiverAddress, string sender);
    }
}
