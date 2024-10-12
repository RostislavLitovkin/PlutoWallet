using Substrate.NetApi;
using System.Numerics;

namespace UniqueryPlusTests
{
    class UniqueSellEVM
    {
        [Test]
        [TestCase(819u, 1u, 200u)]
        [TestCase(813u, 0u, null)]
        public async Task PriceOfNftAsync(uint collectionId, uint id, uint? expectedPrice)
        {
            var price = await UniqueryPlus.Nfts.UniqueNftModel.GetNftPriceAsync(collectionId, id);

            Assert.That(price, Is.EqualTo(expectedPrice is not null ? (BigInteger)(expectedPrice * 1e18) : null));
        }

        [Test]
        public void GetBuyEVMFunctionEncoded()
        {
            var bytes = UniqueryPlus.Nfts.UniqueNftModel.GetBuyEVMFunctionEncoded(100, 0, "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y");

            Console.WriteLine(bytes);
        }

        [Test]
        public void GetSellEVMFunctionEncoded()
        {
            // Taken from: https://unique.subscan.io/extrinsic/5908919-2
            var expectedCalldata = "0xc81b8ef40000000000000000000000000000000000000000000000000000000000000333000000000000000000000000000000000000000000000000000000000000000100000000000000000000000000000000000000000000000ad78ebc5ac6200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000c4a6d6f96eaa847679b5ee29b145f0a658f40da729da2ed2c8ff26b49fe4d57b";

            var bytes = UniqueryPlus.Nfts.UniqueNftModel.GetSellEVMFunctionEncoded(819, 1, BigInteger.Parse("200000000000000000000"), "unixuHLc4UoAjwLpkQHUWy2NpT5LV4tUqJFnFVNyLaeqBfq22");

            Console.WriteLine(Utils.Bytes2HexString(bytes).ToLower());

            Assert.That(Utils.Bytes2HexString(bytes).ToLower(), Is.EqualTo(expectedCalldata));
        }
    }
}
