
using System.Numerics;

namespace UniqueryPlus.Collections
{
    public interface ICollectionBase
    {
        public NftTypeEnum Type { get; }
        public BigInteger CollectionId { get; set; }
        public string Owner { get; set; }
        public uint NftCount { get; set; }
        public ICollectionMetadataBase? Metadata { get; set; }
        public Task<IEnumerable<object>> GetNftsAsync(int limit, byte[]? lastKey);
    }
}
