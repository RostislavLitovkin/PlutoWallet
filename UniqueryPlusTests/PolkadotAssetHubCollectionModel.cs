using PolkadotAssetHub.NetApi.Generated.Model.sp_core.crypto;
using PolkadotAssetHub.NetApi.Generated.Storage;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types.Primitive;
using PolkadotAssetHub.NetApi.Generated;
using UniqueryPlus.Collections;
using UniqueryPlus;
using System.Numerics;

namespace UniqueryPlusTests
{
    internal class PolkadotAssetHubCollectionModelTests
    {
        private const string address = "5DAM8XCuWwxkh42NFBXaAnH6v7jYbd3uQjVKkLPre5LTtmTL";

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private SubstrateClientExt client;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public async Task SetupAsync()
        {
            client = new SubstrateClientExt(new Uri("wss://dot-rpc.stakeworld.io/assethub"), default);

            await client.ConnectAsync();
        }
        /// <summary>
        /// Checks for compile time error in case of a runtime upgrade and any change in the storage structure.
        /// </summary>
        [Test]
        public void TestCollectionAccountStorage()
        {
            var account32 = new AccountId32();
            account32.Create(Utils.GetPublicKeyFrom("5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y"));

            var key = NftsStorage.CollectionAccountParams(new Substrate.NetApi.Model.Types.Base.BaseTuple<PolkadotAssetHub.NetApi.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U32>(account32, new U32(0)));

            Assert.AreEqual("0xE8D49389C2E23E152FDD6364DAADD2CCBA8A0A83BC67004515861C085971C0E4A74DF539A5106A02FCF79CE8C30EF10E6A4E76D530FA715A95388B889AD33C1665062C3DEC9BF0ACA3A9E4FF45781E4811D2DF4E979AA105CF552E9544EBD2B500000000", key.ToString());
        }

        [Test]
        public async Task TestGetCollectionsByOwnedAsync()
        {
            var collections = await CollectionModel.GetCollectionsOwnedByAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, address, 10, null, CancellationToken.None);

            Assert.That(collections.Items.Count(), Is.GreaterThanOrEqualTo(1));

            var firstCollection = collections.Items.First();

            Assert.That(firstCollection.Owner, Is.EqualTo(address));
            Assert.That(firstCollection.NftCount, Is.GreaterThanOrEqualTo(17));
            Assert.That(firstCollection.Metadata, Is.Not.Null);
            Assert.That(firstCollection.CollectionId, Is.EqualTo((BigInteger)208));

            foreach(var collection in collections.Items)
            {
                Assert.That(collection.Metadata?.Name, Is.EqualTo("Double Pendulum"));
                Console.WriteLine(collection.Metadata?.Description);
                Assert.That(collection.Metadata?.Image, Is.EqualTo($"{UniqueryPlus.Constants.KODA_IPFS_ENDPOINT}bafkreiev3vnvnqcyygjqwalwajgnqfzl5ywwud7wy3yhtpxnql5joyxnte"));
            }

            #region GetFirst3Nfts
            var first3Nfts = await firstCollection.GetNftsAsync(3, null, CancellationToken.None);

            Assert.That(first3Nfts.Count(), Is.EqualTo(3));

            foreach (var nft in first3Nfts)
            {
                Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                Assert.That(nft.Metadata?.Description, Is.Not.Null);
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }
            #endregion

            #region GetFirst3Nfts owned by
            var ownedBy = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

            var first3NftsOwnedBy = await firstCollection.GetNftsOwnedByAsync(ownedBy, 3, null, CancellationToken.None);

            Assert.That(first3NftsOwnedBy.Count(), Is.EqualTo(3));

            foreach (var nft in first3NftsOwnedBy)
            {
                Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                Assert.That(nft.Metadata?.Description, Is.Not.Null);
                Assert.That(nft.Owner, Is.EqualTo(ownedBy));
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }

            var otherFirst3NftsOwnedBy = await firstCollection.GetNftsOwnedByAsync(address, 3, null, CancellationToken.None);

            Assert.That(otherFirst3NftsOwnedBy.Count(), Is.EqualTo(3));

            foreach (var nft in otherFirst3NftsOwnedBy)
            {
                Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                Assert.That(nft.Metadata?.Description, Is.Not.Null);
                Assert.That(nft.Owner, Is.EqualTo(address));
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }
            #endregion

            #region GetFullCollection

            var fullCollection = await firstCollection.GetFullAsync();

            Assert.That(fullCollection is ICollectionMintConfig);

            var mintConfig = fullCollection as ICollectionMintConfig;

            Assert.That(mintConfig.NftMaxSuply, Is.Null);
            Assert.That(mintConfig.MintStartBlock, Is.Null);
            Assert.That(mintConfig.MintEndBlock, Is.Null);
            Assert.That(mintConfig.MintType.Type, Is.EqualTo(MintTypeEnum.Public));
            Assert.That(mintConfig.MintPrice, Is.EqualTo((BigInteger)2000000000));

            #endregion
        }

        [Test]
        public async Task TestGetCollectionByCollectionIdAsync()
        {
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, 82, CancellationToken.None);

            Assert.That(collection.Metadata?.Name, Is.EqualTo("Alchemy"));
            Console.WriteLine(collection.Metadata?.Description);
            Assert.That(collection.Metadata?.Image, Is.EqualTo($"{UniqueryPlus.Constants.KODA_IPFS_ENDPOINT}bafkreifb6mmup67vdnbz76gmck27salfjpgt57xalspgcxpsm3fplekb7i"));

            #region GetFullCollection

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

            // As this is testing the package on real data, the actual value can change, thus breaking this test. Some other values close to it will be correct :)
            Assert.That(collectionStats.HighestSale, Is.EqualTo(BigInteger.Parse("330000000000")));

            // As this is testing the package on real data, the actual value can change, thus breaking this test. Some other values close to it will be correct :)
            Assert.That(collectionStats.FloorPrice, Is.EqualTo(BigInteger.Parse("5000000000")));

            // As this is testing the package on real data, the actual value can change, thus breaking this test. Some other values close to it will be correct :)
            Assert.That(collectionStats.Volume, Is.EqualTo(BigInteger.Parse("2409588099999")));
            #endregion
        }

        [Test]
        public async Task TestGetTotalCountOfCollectionsAsync()
        {
            var numberOfCollections = await CollectionModel.GetTotalCountOfCollectionsAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, CancellationToken.None);

            Assert.That(numberOfCollections, Is.GreaterThan(200));

            Console.WriteLine(numberOfCollections);
        }

        [Test]
        public async Task TestTotalCountOfCollectionsForSaleAsync()
        {
            var numberOfCollections = await CollectionModel.GetTotalCountOfCollectionsForSaleAsync(NftTypeEnum.PolkadotAssetHub_NftsPallet, CancellationToken.None);

            Assert.That(numberOfCollections, Is.GreaterThan(100));

            Console.WriteLine(numberOfCollections);
        }

        [Test]
        public async Task TestCollectionsForSaleAsync()
        {
            var collectionsEnumerable = CollectionModel.GetCollectionsForSaleAsync([client], 3);

            var enumerator = collectionsEnumerable.GetAsyncEnumerator();

            for (uint i = 0; i < 10; i++)
            {
                if (await enumerator.MoveNextAsync())
                {
                    var collection = enumerator.Current;
                    Console.WriteLine($"{collection.CollectionId} - {collection.Metadata?.Name} owned by {collection.Owner}");
                    Console.WriteLine("Image: " + collection.Metadata?.Image);
                }
            }
        }

        [Test]
        public async Task TestRandomCollectionsForSaleAsync()
        {
            var randomCollections1 = await CollectionModel.GetRandomCollectionsForSaleAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, 3);

            var randomCollections2 = await CollectionModel.GetRandomCollectionsForSaleAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, 3);

            foreach (var collection in randomCollections1)
            {
                Console.WriteLine($"{collection.CollectionId} - {collection.Metadata?.Name} owned by {collection.Owner}");
            }

            Assert.That(randomCollections1.First().Metadata?.Name, Is.Not.EqualTo(randomCollections2.First().Metadata?.Name));
        }
    }
}
