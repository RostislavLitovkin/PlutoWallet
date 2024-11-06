using Newtonsoft.Json;

namespace UniqueryPlus.Metadata
{
    public record MetadataAttribute
    {
        [JsonProperty("trait_type")]
        public required string TraitType { get; set; }

        [JsonProperty("value")]
        public required string Value { get; set; }
    }
}
