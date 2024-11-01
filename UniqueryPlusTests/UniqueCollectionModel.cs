
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
            Assert.That(collection.NftCount, Is.EqualTo(2));
            Assert.That(collection.Owner, Is.EqualTo(otherAddress));

            #region GetNfts

            var nfts = await ((UniqueCollection)collection).GetNftsAsync(3, null, CancellationToken.None);
            Assert.That(nfts.Count(), Is.EqualTo(2));

            foreach (var nft in nfts)
            {
                Assert.That(nft.Metadata?.Name, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Description, Is.EqualTo("xx"));
                Assert.That(nft.CollectionId, Is.EqualTo((BigInteger)50));
                Assert.That(nft.Owner, Is.EqualTo(otherAddress));
            }
            Assert.That(nfts.First().Metadata?.Image, Is.EqualTo($"{Constants.UNIQUE_IPFS_ENDPOINT}Qmce9jbJqrf5BnK2zSY8kiNXKJLhoJz36gDRvCiJTRACcx"));


            nfts = await collection.GetNftsOwnedByAsync(otherAddress, 3, null, CancellationToken.None);
            Assert.That(nfts.Count(), Is.EqualTo(2));

            foreach (var nft in nfts)
            {
                Assert.That(nft.Metadata?.Name, Is.EqualTo("xx"));
                Assert.That(nft.Metadata?.Description, Is.EqualTo("xx"));
                Assert.That(nft.CollectionId, Is.EqualTo((BigInteger)50));
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

        [Test]
        [TestCase(725u, "Lolo7406", "Test ", "https://ipfs.unique.network/ipfs/QmdTcA8BZ46miup22PpSLZuy3VhgkYjXLZc9CANWLkViG1", 2u, "5DPvknj5rVqKzdCM7F5JQedejSWaNfDmc9Yq6rfQQ2W8UjoL")]
        [TestCase(763u, "Cute Animal Fashionverse", "Cute Animal Fashionverse", "https://ipfs.unique.network/ipfs/QmSKBCArJN36f1f3Z5RerUoaG8ezoXghjJzxaPFJqh2M7u", 6u, "5FsmmJTntP48C4LxhL4NgmHmpsWdHRkYuzbMLimd66mwZ7cL")]
        [TestCase(470u, "Curso DeFi en Polkadot v1 2024", "Certificado de participación en el curso DeFi en Polkadot, dictado el primer trimestre del 2024, con una duración de 10 sesiones teóricas y prácticas. Se estudiaron los protocolos DeFi en el ecosistema Polkadot.", "https://ipfs.unique.network/ipfs/QmStAiwAGN5ZKDefmuL2gXNxffDn3hwi3wEEJPd2FdK1hh", 77u, "5FHMbBzwFaXRygShrLLGVcT4siWwYAekfDibYKJkfQ58h9Tc")]
        public async Task TestGetCollectionByCollectionIdAsync(uint collectionId, string name, string description, string ipfsImage, uint nftCount, string owner)
        {
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.Unique, collectionId, CancellationToken.None);

            Assert.That(collection, Is.Not.Null);

            Assert.That(collection.Metadata?.Name, Is.EqualTo(name));
            Assert.That(collection.Metadata?.Description, Is.EqualTo(description));
            Assert.That(collection.Metadata?.Image, Is.EqualTo(ipfsImage));
            Assert.That(collection.CollectionId, Is.EqualTo((BigInteger)collectionId));
            Assert.That(collection.NftCount, Is.EqualTo(nftCount));
            Assert.That(collection.Owner, Is.EqualTo(owner));
        }
    }
}
