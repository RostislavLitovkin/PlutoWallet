using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Nfts
{
    public interface INftTransferable
    {
        public Method Transfer(string recipientAddress);
    }
}
