namespace UniqueryPlus.Metadata
{
    public record MetadataWithBaseUri
    {
        public required string Name { get; init; }
        public required string BaseUri { get; init; }
    }
}
