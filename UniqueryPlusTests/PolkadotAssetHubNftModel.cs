using PolkadotAssetHub.NetApi.Generated.Storage;
using Substrate.NetApi.Model.Types.Primitive;
using UniqueryPlus;
using PolkadotAssetHub.NetApi.Generated;
using UniqueryPlus.Nfts;


namespace UniqueryPlusTests
{
    internal class PolkadotAssetHubNftModel
    {
        private const string address = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

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
        public void TestItemStorage()
        {
            var key = NftsStorage.ItemParams(new Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32>(new U32(0), new U32(0)));

            Console.WriteLine(key);

            Assert.That(key, Is.EqualTo("0xE8D49389C2E23E152FDD6364DAADD2CCE6636C7091B616C035068763633F33E211D2DF4E979AA105CF552E9544EBD2B50000000011D2DF4E979AA105CF552E9544EBD2B500000000"));
        }

        /// <summary>
        /// Checks for compile time error in case of a runtime upgrade and any change in the storage structure.
        /// </summary>
        [Test]
        public void TestItemMetadataOfStorage()
        {
            var key = NftsStorage.ItemMetadataOfParams(new Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32>(new U32(0), new U32(0)));

            Assert.That(key, Is.EqualTo("0xE8D49389C2E23E152FDD6364DAADD2CC0DD0A9A990C376382ED287D43F24285C11D2DF4E979AA105CF552E9544EBD2B50000000011D2DF4E979AA105CF552E9544EBD2B500000000"));
        }

        [Test]
        public async Task TestGetNftsByOwned()
        {
            var first3Nfts = await NftModel.GetNftsOwnedByAsync(client, NftTypeEnum.PolkadotAssetHub_NftsPallet, address, 3, null, CancellationToken.None);

            Assert.That(first3Nfts.Items.Count(), Is.EqualTo(3));

            foreach (var nft in first3Nfts.Items)
            {
                Console.WriteLine($"{nft.Id} - {nft.Metadata?.Name} owned by {nft.Owner}");
                Assert.That(nft.Metadata?.Description, Is.Not.Null);
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }
        }
    }
}
