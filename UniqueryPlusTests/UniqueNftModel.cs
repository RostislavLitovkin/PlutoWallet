using Unique.NetApi.Generated;
using UniqueryPlus.Nfts;
using UniqueryPlus;
using UniqueryPlus.Collections;
using System.Numerics;

namespace UniqueryPlusTests
{
    internal class UniqueNftModel
    {
        private const string address = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

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
        [Ignore("This test is very slow")]
        public async Task TestGetNftsByOwnedOnChainAsync()
        {
            var first3Nfts = await NftModel.GetNftsOwnedByOnChainAsync(client, NftTypeEnum.Unique, address, 3, null, CancellationToken.None);

            Assert.That(first3Nfts.Items.Count(), Is.EqualTo(3));

            foreach (var nft in first3Nfts.Items)
            {
                Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                Assert.That(nft.Metadata?.Description, Is.Not.Null);
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }

            var fullNft = await first3Nfts.Items.Last().GetFullAsync(CancellationToken.None);

            Assert.That(fullNft is INftSellable);

            var sellable = (INftSellable)fullNft;

            Assert.That(sellable.Price, Is.Null);
        }

        /// Careful about this bug: https://github.com/SubstrateGaming/Substrate.NET.Toolchain/issues/85
        [Test]
        [TestCase("5DkMtAbCBBBkycT5Gowo2ddkkmf7nVV1TW62fVZeSDLqRtEc")]
        [TestCase(address)]
        public async Task TestGetNftsByOwnedAsync(string address)
        {
            var nftsEnumerable = NftModel.GetNftsOwnedByAsync([client], address, 3);

            var enumerator = nftsEnumerable.GetAsyncEnumerator();

            for (uint i = 0; i < 10; i++)
            {
                if (await enumerator.MoveNextAsync())
                {
                    var nft = enumerator.Current;
                    Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                    Console.WriteLine("Image: " + nft.Metadata?.Image);

                    var collection = await ((UniqueNft)nft).GetCollectionAsync(CancellationToken.None);

                    Assert.That(collection.CollectionId, Is.EqualTo(nft.CollectionId));
                    Console.WriteLine("Collection id: " + collection.CollectionId);
                    Console.WriteLine("Collection cover image: " + collection.Metadata?.Image);
                }
            }
        }

        [Test]
        [TestCase(304u)]
        public async Task TestGetNestedNftsAsync(uint collectionId)
        {
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.Unique, collectionId, CancellationToken.None);

            Console.WriteLine(collection.Metadata.Name);
            Console.WriteLine(collection.NftCount);

            Assert.That(collection.CollectionId, Is.EqualTo((BigInteger)collectionId));

            var nfts = await ((UniqueCollection)collection).GetNftsAsync(25, null, CancellationToken.None);

            Console.WriteLine("Nfts count: " + nfts.Count());

            foreach (var nft in nfts)
            {
                Console.WriteLine("Name: " + nft.Metadata.Name);
                Console.WriteLine("Image: " + nft.Metadata.Image);
                Console.WriteLine("Description: " + nft.Metadata.Description);
                Console.WriteLine("Id: " + nft.Id);
            }
        }
    }
}
