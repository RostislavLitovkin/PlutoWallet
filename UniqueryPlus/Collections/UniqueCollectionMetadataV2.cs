using Newtonsoft.Json;

namespace UniqueryPlus.Collections
{
    internal record UniqueCollectionMetadataV2
    {
        [JsonProperty("cover_image")]
        public required UniqueCollectionMetadataV2Image CoverImage { get; init; }
    }

    internal record UniqueCollectionMetadataV2Image
    {
        [JsonProperty("url")]
        public required string Url { get; init; }
    }
}
