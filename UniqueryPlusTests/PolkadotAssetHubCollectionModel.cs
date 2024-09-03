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
        private const string address = "126eGrTyNjEE8b2tCpaaJw7FmjjCHvc3VEDoudPDCAMz55Kz";

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
        public async Task TestGetCollectionsByOwned()
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
                Console.WriteLine(collection.Metadata?.Name);
                Console.WriteLine(collection.Metadata?.Description);
                Console.WriteLine("Image: " + collection.Metadata?.Image);
            }
        }
    }
}
