using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Substrate.NetApi;
using Uniquery.Types;

namespace PlutoWallet.Model
{
    public class RmrkAttributes
    {
        [JsonPropertyName("trait_type")]
        public string TraitType { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("display")]
        public string Display { get; set; }
    }

    public class RmrkMetadata
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("animation_url")]
        public string AnimationUrl { get; set; }

        [JsonPropertyName("attributes")]
        public List<RmrkAttributes> Attributes { get; set; }

        [JsonPropertyName("external_url")]
        public string ExternalUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class UniqueryModel
	{
		public static async Task<List<NFT>> GetAccountRmrk()
		{
			string address = Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), 2);

			List<NftEntity> entities = await Uniquery.Uniquery.GetAccountRmrk(address);
			List<NFT> rmrks = new List<NFT>();

			foreach (NftEntity entity in entities)
			{
                string metadataJson = await Model.IpfsModel.FetchIpfsAsync(entity.Metadata);

                RmrkMetadata metadata = JsonConvert.DeserializeObject<RmrkMetadata>(metadataJson);

                NFT nft = new NFT
                {
                    Name = metadata.Name,
                    Description = metadata.Description,
                    Image = Model.IpfsModel.ToIpfsLink(metadata.Image),
                    AnimationUrl = metadata.AnimationUrl,
                    Attributes = new string[1] { metadata.Attributes[0].Value },
                    ExternalUrl = metadata.ExternalUrl,
                    Type = metadata.Type,

                };
                

				rmrks.Add(nft);
			}

			return rmrks;
        }
	}
}

