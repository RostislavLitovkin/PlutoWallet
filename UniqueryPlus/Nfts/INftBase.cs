using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.Metadata;

namespace UniqueryPlus.Nfts
{
    public interface INftBase
    {
        public NftTypeEnum Type { get; }
        public BigInteger CollectionId { get; set; }
        public BigInteger Id { get; set; }
        public string Owner { get; set; }
        public MetadataBase? Metadata { get; set; }
        public Task<ICollectionBase> GetCollectionAsync(CancellationToken token);
        public Task<INftBase> GetFullAsync(CancellationToken token);
    }
}
