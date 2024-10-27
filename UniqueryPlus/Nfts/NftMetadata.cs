
using UniqueryPlus.Ipfs;

namespace UniqueryPlus.Nfts
{
    public record NftMetadata : INftMetadataBase, IMetadataImage
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public Attribute[]? Attributes { get; set; }
    }
}
