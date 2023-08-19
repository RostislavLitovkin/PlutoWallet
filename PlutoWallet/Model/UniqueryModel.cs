using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Substrate.NetApi;
using PlutoWallet.Constants;

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
        private static Endpoint GetEndpointFromFormat(string nftFormat)
        {
            switch (nftFormat)
            {
                case "rmrk":
                    Endpoint rmrk = Endpoints.GetEndpointDictionary["kusama"];
                    rmrk.Name = "Rmrk";
                    return rmrk;
                case "rmrk2":
                    Endpoint rmrk2 = Endpoints.GetEndpointDictionary["kusama"];
                    rmrk2.Name = "Rmrk2";
                    return rmrk2;
                case "basilisk":
                    return Endpoints.GetEndpointDictionary["basilisk"];
                case "glmr":
                    return Endpoints.GetEndpointDictionary["moonbeam"];
                case "movr":
                    return Endpoints.GetEndpointDictionary["moonriver"];
                case "unique":
                    return Endpoints.GetEndpointDictionary["unique"];
                case "quartz":
                    return Endpoints.GetEndpointDictionary["quartz"];
                case "opal":
                    return Endpoints.GetEndpointDictionary["opal"];
                default:
                    return null;
            }
        }

        public static async Task AddRmrkNfts(Action<List<NFT>> updateNfts)
        {
            updateNfts.Invoke(await GetAccountRmrk());
        }

        public static async Task<List<NFT>> GetAccountRmrk()
		{
			string address = Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), 2);

			List<NFT> rmrks = new List<NFT>();

            var nfts = await Uniquery.Universal.NftListByOwner(KeysModel.GetSubstrateKey(), 100, eventsLimit: 0);

            foreach (var nft in nfts)
            {
                rmrks.Add(new NFT
                {
                    Name = nft.Name,
                    Description = nft.Meta.Description,
                    Image = IpfsModel.ToIpfsLink(nft.Meta.Image),
                    Endpoint = GetEndpointFromFormat(nft.NetworkFormat),
                });
            }

			return rmrks;
        }
	}
}

