using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unique.NetApi.Generated;
using UniqueryPlus.Nfts;
using UniqueryPlus;

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
        public async Task TestGetNftsByOwnedAsync()
        {
            var first3Nfts = await NftModel.GetNftsOwnedByAsync(client, NftTypeEnum.Unique, address, 3, null, CancellationToken.None);

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
    }
}
