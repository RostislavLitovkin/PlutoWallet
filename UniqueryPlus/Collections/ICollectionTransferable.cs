using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Collections
{
    public interface ICollectionTransferable
    {
        public bool IsTransferable { get; set; }
        public Method Transfer(string recipientAddress);
    }
}
