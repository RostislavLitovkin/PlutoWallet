namespace UniqueryPlus.Metadata
{
    public interface IMetadataBase : IMetadataImage
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public MetadataAttribute[]? Attributes { get; set; }
    }
}
