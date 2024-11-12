
using System.Numerics;
using UniqueryPlus.Metadata;
using UniqueryPlus.Nfts;

namespace UniqueryPlus.Collections
{
    public interface ICollectionBase
    {
        public NftTypeEnum Type { get; }
        public BigInteger CollectionId { get; set; }
        public string Owner { get; set; }
        public uint NftCount { get; set; }
        public MetadataBase? Metadata { get; set; }
        public Task<IEnumerable<INftBase>> GetNftsAsync(uint limit, byte[]? lastKey, CancellationToken token);
        public Task<IEnumerable<INftBase>> GetNftsOwnedByAsync(string owner, uint limit, byte[]? lastKey, CancellationToken token);
        public Task<ICollectionBase> GetFullAsync(CancellationToken token);
    }
}
