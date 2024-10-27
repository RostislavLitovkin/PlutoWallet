using Newtonsoft.Json;

namespace UniqueryPlus
{
    public record Attribute
    {
        [JsonProperty("trait_type")]
        public required string TraitType { get; set; }

        [JsonProperty("value")]
        public required string Value { get; set; }
    }
}
