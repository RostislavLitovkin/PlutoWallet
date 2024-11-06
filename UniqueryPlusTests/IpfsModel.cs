namespace UniqueryPlusTests
{
    internal class IpfsModel
    {
        [Test]
        [Ignore("This test is not working at this moment")]
        [TestCase("https://imagedelivery.net/jk5b6spi_m_-9qC4VTnjpg/bafkreifb6mmup67vdnbz76gmck27salfjpgt57xalspgcxpsm3fplekb7i/public", UniqueryPlus.Metadata.ImageTypeEnum.Image)]
        [TestCase("https://imagedelivery.net/jk5b6spi_m_-9qC4VTnjpg/bafybeig63doheokjpqkdnfrf3rnyfjirmk3sxxnzxo2mva3asni7vaxjni/public", UniqueryPlus.Metadata.ImageTypeEnum.Image)]
        [TestCase("https://image.w.kodadot.xyz/ipfs/QmYJtZ3bAKQ59o2DLdUx7N9jViHsD3vGXuVzEDEpk9SrLt", UniqueryPlus.Metadata.ImageTypeEnum.Pdf)]
        [TestCase("https://image.w.kodadot.xyz/ipfs/bafkreieixya7a55vfy675sqsfvaaufiow5ygg4nnxp4roqfgpanottzjdi", UniqueryPlus.Metadata.ImageTypeEnum.Video)]
        public async Task GetImageTypeAsync(string ipfsLink, UniqueryPlus.Metadata.ImageTypeEnum expectedType)
        {
            var type = await UniqueryPlus.Ipfs.IpfsModel.GetImageTypeAsync(ipfsLink);

            Console.WriteLine(type);

            Assert.That(type, Is.EqualTo(expectedType));
        }
    }
}
