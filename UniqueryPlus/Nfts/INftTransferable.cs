using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Nfts
{
    public interface INftTransferable
    {
        public bool IsTransferable { get; set; }
        public Method Transfer(string recipientAddress);
    }
}
