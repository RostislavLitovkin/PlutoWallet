using Mythos.NetApi.Generated;
using UniqueryPlus.Nfts;
using UniqueryPlus;
using Nethereum.Contracts.Standards.ERC20.TokenList;
using System.Numerics;

namespace UniqueryPlusTests
{

    internal class MythosNftModel
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
        public async Task TestGetNftsByOwnedOnChainAsync()
        {
            var first3Nfts = await NftModel.GetNftsOwnedByOnChainAsync(client, NftTypeEnum.Mythos, address, 100, null, CancellationToken.None);

            foreach (var nft in first3Nfts.Items)
            {
                Console.WriteLine($"{nft.CollectionId}-{nft.Id}: {nft.Metadata?.Name} owned by {nft.Owner}");
                Console.WriteLine(nft.Metadata?.Description);
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }

            var fullNft = await first3Nfts.Items.Last().GetFullAsync(CancellationToken.None);

            Assert.That(fullNft is INftSellable);
        }

        [Test]
        public async Task GetNftByIdAsync()
        {
            CancellationToken token = CancellationToken.None;
            (BigInteger, BigInteger)[] mythosIds = [(BigInteger.Parse("86219270927352332455509372315086258213278212512"), new BigInteger(101))];

            foreach (var id in mythosIds)
            {
                var nft = await UniqueryPlus.Nfts.NftModel.GetNftByIdAsync(client, NftTypeEnum.Mythos, id.Item1, id.Item2, token).ConfigureAwait(false);

                Console.WriteLine($"{nft.CollectionId}-{nft.Id}: {nft.Metadata?.Name} owned by {nft.Owner}");
                Console.WriteLine(nft.Metadata?.Description);
                Console.WriteLine("Image: " + nft.Metadata?.Image);
            }
        }
    }
}
