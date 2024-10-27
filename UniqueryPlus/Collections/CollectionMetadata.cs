using UniqueryPlus.Ipfs;

namespace UniqueryPlus.Collections
{
    public record CollectionMetadata : ICollectionMetadataBase, IMetadataImage
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public Attribute[]? Attributes { get; set; }
    }
}
