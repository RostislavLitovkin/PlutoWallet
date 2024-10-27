
namespace UniqueryPlus.Nfts
{
    public interface INftMetadataBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public Attribute[]? Attributes { get; set; }
    }
}
