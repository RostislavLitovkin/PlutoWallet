using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Substrate.NetApi;
using PlutoWallet.Constants;
using Uniquery;
using UniqueryPlus.Ipfs;

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
                    Endpoint rmrk = Endpoints.GetEndpointDictionary[EndpointEnum.Kusama].Clone();
                    rmrk.Name = "Rmrk";
                    return rmrk;
                case "rmrk2":
                    Endpoint rmrk2 = Endpoints.GetEndpointDictionary[EndpointEnum.Kusama].Clone();
                    rmrk2.Name = "Rmrk2";
                    return rmrk2;
                case "basilisk":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Basilisk];
                case "glmr":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Moonbeam];
                case "movr":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Moonriver];
                case "unique":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Unique];
                case "quartz":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Quartz];
                case "opal":
                    return Endpoints.GetEndpointDictionary[EndpointEnum.Opal];
                default:
                    return null;
            }
        }

        public static async Task<List<NFT>> GetAllNfts(string substrateAddress, CancellationToken token)
        {

            List<Nft> nfts = new List<Nft>();
            int limit = 100;
            int offset = 0;
            string orderBy = "updatedAt_DESC";
            bool forSale = false;
            int eventsLimit = 0;

            try
            {
                nfts.AddRange(await Rmrk.NftListByOwner(substrateAddress, limit, offset, orderBy, forSale, eventsLimit, 10, token));
            }
            catch
            {

            }

            try
            {
                nfts.AddRange(await RmrkV2.NftListByOwner(substrateAddress, limit, offset, orderBy, forSale, eventsLimit, 10, token));
            }
            catch
            {

            }

            try
            {

                nfts.AddRange(await Unique.NftListByOwner(substrateAddress, limit, offset, token));
            }
            catch
            {

            }

            try
            {
                nfts.AddRange(await Quartz.NftListByOwner(substrateAddress, limit, offset, token));
            }
            catch
            {

            }

            // NFTs will be fixed soon in the next update..
            //nfts.AddRange(await Opal.NftListByOwner(substrateAddress, limit, offset, token));

            try
            {
                nfts.AddRange(await Basilisk.NftListByOwner(substrateAddress, limit, offset, orderBy, forSale, eventsLimit, 10, token));
            }
            catch { }

            try
            {
                nfts.AddRange(await Glmr.NftListByOwner(substrateAddress, limit, offset, orderBy, forSale, eventsLimit, token));
            }
            catch
            {

            }
            try
            {
                nfts.AddRange(await Movr.NftListByOwner(substrateAddress, limit, offset, orderBy, forSale, eventsLimit, token));
            }
            catch
            {

            }
            try
            {
                nfts.AddRange(await Acala.NftListByOwner(substrateAddress, limit, offset, token));
            }
            catch
            {

            }

            try
            {
                nfts.AddRange(await Astar.NftListByOwner(substrateAddress, limit, offset, token));
            }
            catch
            {
            }

            try
            {
                nfts.AddRange(await Shiden.NftListByOwner(substrateAddress, limit, offset, token));
            }
            catch
            {

            }

            List<NFT> result = new List<NFT>();
            foreach (var nft in nfts)
            {
                result.Add(new NFT
                {
                    Name = nft.Name,
                    Description = nft.Meta.Description,
                    Image = IpfsModel.ToIpfsLink(nft.Meta.Image),
                    Endpoint = GetEndpointFromFormat(nft.NetworkFormat),
                });
            }

            return result;
        }
    }
}

