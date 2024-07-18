using Newtonsoft.Json;

namespace PlutoWallet.Model.HydrationModel
{
    public class HydrationAsset
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("decimals")]
        public int Decimals { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("isSufficient")]
        public bool IsSufficient { get; set; }

        [JsonProperty("existentialDeposit")]
        public string ExistentialDeposit { get; set; }
    }
}
