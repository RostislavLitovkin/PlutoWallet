﻿using PlutoWallet.Constants;
using System.Numerics;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using UniqueryPlus;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Model
{
    public class MockNft : INftBase, IKodaLink
    {
        public NftTypeEnum Type => NftTypeEnum.PolkadotAssetHub_NftsPallet;
        public required BigInteger CollectionId { get; set; }
        public required BigInteger Id { get; set; }
        public required string Owner { get; set; }
        public INftMetadataBase? Metadata { get; set; }
        public string KodaLink => $"https://koda.art/ahp/nft/{CollectionId}/{Id}";
        public async Task<ICollectionBase> GetCollectionAsync(CancellationToken token)
        {
            return CollectionModel.GetMockCollection();
        }

        public async Task<INftBase> GetFullAsync(CancellationToken token)
        {
            return this;
        }
    }
    public class NftWrapper
    {
        public required INftBase NftBase { get; set; }
        public required Endpoint Endpoint { get; set; }
        public bool Favourite { get; set; } = false;

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(NftWrapper)))
            {
                return false;
            }

            var objNft = (NftWrapper)obj;

            return (objNft.NftBase.Metadata?.Name == this.NftBase.Metadata?.Name &&
                objNft.NftBase.Metadata?.Description == this.NftBase.Metadata?.Description &&
                objNft.NftBase.Metadata?.Image == this.NftBase.Metadata?.Image &&
                objNft.Endpoint.Key == this.Endpoint.Key);
        }

        public override string ToString()
        {
            return this.NftBase.Metadata?.Name + " - " + this.NftBase.Metadata?.Image;
        }
    }
    public class NftModel
    {
        public static INftBase GetMockNft(
            string name = "Mock Nft",
            string imageSource = "darkbackground2.png"
        )
        {
            Random random = new Random();

            int digits = random.Next(1, 9);

            return new MockNft
            {
                CollectionId = random.Next((int)Math.Pow(10, digits)),
                Id = random.Next((int)Math.Pow(10, digits)),
                Metadata = new NftMetadata
                {
                    Name = name,
                    Description = "Welcome, this is a mock Nft to test the UI for Nft views even without an internet connection. Yes, it is pretty handy! I need to make the description a little bit longer to test the edge cases with having a text to go out of bounds of the visible View.",
                    Image = imageSource,
                },
                Owner = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"
            };
        }

        public static NftWrapper ToNftWrapper(INftBase nft)
        {
            return new NftWrapper
            {
                NftBase = nft,
                Endpoint = nft.Type switch
                {
                    NftTypeEnum.PolkadotAssetHub_NftsPallet => Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub],
                    NftTypeEnum.KusamaAssetHub_NftsPallet => Endpoints.GetEndpointDictionary[EndpointEnum.KusamaAssetHub],
                    NftTypeEnum.Unique => Endpoints.GetEndpointDictionary[EndpointEnum.Unique],
                    _ => throw new NotImplementedException(),
                }
            };
        }
    }
}
