
using System.Numerics;
using Unique.NetApi.Generated;
using UniqueryPlus.Collections;
using UniqueryPlus;

namespace UniqueryPlusTests
{
    internal class UniqueCollectionModel
    {
        private const string address = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

        private const string otherAddress = "5CQFufCUmoGJKwscyMZVcjKtmktKNzXPnxd8NEH4ibRN3T9o";

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private SubstrateClientExt client;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public async Task SetupAsync()
        {
            client = new SubstrateClientExt(new Uri("wss://eu-ws.unique.network"), default);

            await client.ConnectAsync();
        }

        [Test]
        public async Task TestGetCollectionByCollectionIdAsync()
        {
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.Unique, 50, CancellationToken.None);

            Assert.That(collection.Metadata?.Name, Is.EqualTo("xx"));
            Assert.That(collection.Metadata?.Description, Is.EqualTo("xx"));
            Assert.That(collection.Metadata?.Image, Is.EqualTo($"{Constants.UNIQUE_IPFS_ENDPOINT}QmfTWMWpi7Qk7E383DVm4SPKzjSKMXRHiCA2Xr9R3kkvTR"));
            Assert.That(collection.CollectionId, Is.EqualTo((BigInteger)50));
            Assert.That(collection.NftCount, Is.EqualTo(1));
            Assert.That(collection.Owner, Is.EqualTo(otherAddress));

            #region GetNfts

            var nfts = await collection.GetNftsAsync(3, null, CancellationToken.None);
            Assert.That(nfts.Count(), Is.EqualTo(1));

            foreach (var nft in nfts)
            {
                Assert.That(nft.Metadata?.Name, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Description, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Image, Is.EqualTo($"{Constants.UNIQUE_IPFS_ENDPOINT}Qmce9jbJqrf5BnK2zSY8kiNXKJLhoJz36gDRvCiJTRACcx"));
                Assert.That(nft.CollectionId, Is.EqualTo((BigInteger)50));
                Assert.That(nft.Id, Is.EqualTo((BigInteger)2));
                Assert.That(nft.Owner, Is.EqualTo(otherAddress));
            }

            nfts = await collection.GetNftsOwnedByAsync(otherAddress, 3, null, CancellationToken.None);
            Assert.That(nfts.Count(), Is.EqualTo(1));

            foreach (var nft in nfts)
            {
                Assert.That(nft.Metadata?.Name, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Description, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Image, Is.EqualTo($"{Constants.UNIQUE_IPFS_ENDPOINT}Qmce9jbJqrf5BnK2zSY8kiNXKJLhoJz36gDRvCiJTRACcx"));
                Assert.That(nft.CollectionId, Is.EqualTo((BigInteger)50));
                Assert.That(nft.Id, Is.EqualTo((BigInteger)2));
                Assert.That(nft.Owner, Is.EqualTo(otherAddress));
            }


            #endregion

            #region GetFullCollection
            /*
            var fullCollection = await collection.GetFullAsync();

            Assert.That(fullCollection is ICollectionMintConfig);

            var mintConfig = fullCollection as ICollectionMintConfig;

            Assert.That(mintConfig.NftMaxSuply, Is.EqualTo(256));
            Assert.That(mintConfig.MintStartBlock, Is.Null);
            Assert.That(mintConfig.MintEndBlock, Is.Null);
            Assert.That(mintConfig.MintType.Type, Is.EqualTo(MintTypeEnum.Public));
            Assert.That(mintConfig.MintPrice, Is.EqualTo(BigInteger.Parse("3000000000")));

            Assert.That(fullCollection is ICollectionCreatedAt);
            var createdAt = fullCollection as ICollectionCreatedAt;
            Assert.That(createdAt.CreatedAt, Is.EqualTo(new DateTimeOffset(2024, 1, 22, 14, 33, 0, default)));

            Assert.That(fullCollection is ICollectionStats);
            var collectionStats = fullCollection as ICollectionStats;
            Assert.That(collectionStats.HighestSale, Is.EqualTo(BigInteger.Parse("330000000000")));
            Assert.That(collectionStats.FloorPrice, Is.EqualTo(BigInteger.Parse("4990000000")));
            Assert.That(collectionStats.Volume, Is.EqualTo(BigInteger.Parse("2404598099999")));
            */
            #endregion
        }

    }
}
