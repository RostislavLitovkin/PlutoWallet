using Substrate.NetApi.Model.Extrinsics;

namespace UniqueryPlus.Collections
{
    public interface ICollectionTransferable
    {
        public Method Transfer(string recipientAddress);
    }
}
