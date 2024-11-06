namespace UniqueryPlus.Metadata
{
    public record MetadataBase : IMetadataBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public ImageTypeEnum UniqueryPlus_ImageType { get; set; }
        public MetadataAttribute[]? Attributes { get; set; }
    }
}
