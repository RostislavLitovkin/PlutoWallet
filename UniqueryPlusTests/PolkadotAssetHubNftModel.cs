using PolkadotAssetHub.NetApi.Generated.Model.sp_core.crypto;
using PolkadotAssetHub.NetApi.Generated.Storage;
using Substrate.NetApi.Model.Types.Primitive;

namespace UniqueryPlusTests
{
    internal class PolkadotAssetHubNftModel
    {
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

    }
}
