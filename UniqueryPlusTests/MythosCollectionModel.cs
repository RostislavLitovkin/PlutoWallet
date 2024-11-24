using UniqueryPlus.Collections;
using UniqueryPlus;
using Mythos.NetApi.Generated;
using System.Numerics;

namespace UniqueryPlusTests
{
    internal class MythosCollectionModel
    {
        private const string address = "0x9932Bf3132Cd3fbdCd5303D5E93Cf392c27BD8fb";

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private SubstrateClientExt client;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public async Task SetupAsync()
        {
            client = new SubstrateClientExt(new Uri("wss://polkadot-mythos-rpc.polkadot.io"), default);

            await client.ConnectAsync();
        }

        [Test]
        public async Task TestGetCollectionByCollectionIdAsync()
        {
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.Mythos, BigInteger.Parse("962395617681360100198543477067988568069845407061"), CancellationToken.None);

            Assert.That(collection.Metadata?.Name, Is.EqualTo("Shakers2024.JonathanGreenard.Luminous"));
            Console.WriteLine(collection.Metadata?.Description);

            #region GetFullCollection

            var fullCollection = await ((MythosCollection)collection).GetFullAsync(CancellationToken.None);

            Assert.That(fullCollection is ICollectionMintConfig);

            var mintConfig = fullCollection as ICollectionMintConfig;

            Assert.That(mintConfig.NftMaxSuply, Is.Null);
            Assert.That(mintConfig.MintStartBlock, Is.Null);
            Assert.That(mintConfig.MintEndBlock, Is.Null);
            Assert.That(mintConfig.MintType.Type, Is.EqualTo(MintTypeEnum.Issuer));
            Assert.That(mintConfig.MintPrice, Is.Null);

            Assert.That(fullCollection is not ICollectionCreatedAt);
            #endregion

            var nfts = await collection.GetNftsAsync(3, null, CancellationToken.None);

            foreach(var nft in nfts)
            {
                Console.WriteLine(nft.Metadata is null);
                Console.WriteLine($"{nft.Metadata?.Name} - {nft.Metadata?.Image}");
                Console.WriteLine($"{nft.Metadata?.Description}");
            }
        }
    }
}
