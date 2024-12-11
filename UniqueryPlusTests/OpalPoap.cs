using Nethereum.JsonRpc.Client;
using Opal.NetApi.Generated;
using System.Numerics;
using UniqueryPlus;
using UniqueryPlus.Collections;
using UniqueryPlus.Nfts;

namespace UniqueryPlusTests
{
    internal class OpalPoap
    {

#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private SubstrateClientExt client;
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        [SetUp]
        public async Task SetupAsync()
        {
            client = new SubstrateClientExt(new Uri("wss://ws-opal.unique.network"), default);

            await client.ConnectAsync();
        }


        [Test()]
        [TestCase("0x17C4E6453Cc49aAAaeaCA894e6d9683e000011cD")]
        public async Task GetPoapEventInfoAsync(string collectionAddress)
        {
            CancellationToken token = CancellationToken.None;
            var collection = await CollectionModel.GetCollectionByCollectionIdAsync(client, NftTypeEnum.Unique, 50, CancellationToken.None);

            Assert.That(collection.Metadata?.Name, Is.EqualTo("Diamonds"));
            Assert.That(collection.Metadata?.Description, Is.EqualTo("Proof-of-everything"));

            var eventInfo = await ((OpalCollectionFull)await collection.GetFullAsync(token)).GetEventInfoAsync(token);

            Assert.That(eventInfo.AccountLimit, Is.EqualTo(new BigInteger(0)));
            Assert.That(eventInfo.TokenImage, Is.EqualTo("https://orange-impressed-bonobo-853.mypinata.cloud/ipfs/QmUXd2duL5S6AvV7r9XKDkCyyuGbRvjjfwDptEyD24WJm7"));

            Console.WriteLine(eventInfo.StartTimestamp);
            Console.WriteLine(eventInfo.EndTimestamp);
            Console.WriteLine(DateTime.UnixEpoch.AddSeconds((long)eventInfo.EndTimestamp));

        }

        [Test]
        public void CheckCollectionIdToCollectionAddressConversion()
        {
            Assert.That(UniqueryPlus.EVM.Helpers.GetCollectionAddress(4557), Is.EqualTo("0x17C4E6453Cc49aAAaeaCA894e6d9683e000011cD".ToLower()));
        }
    }
}
