
using PlutoWallet.Constants;
using PlutoWallet.Model;
using Substrate.NetApi;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests
{
    internal class QueryHistoricalEvents
    {
        SubstrateClient client;

        string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

        [SetUp]
        public async Task Setup()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot];

            var clientExt = new SubstrateClientExt(
                        endpoint,
                        new Uri("wss://rpc.ibp.network/polkadot"),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await clientExt.ConnectAndLoadMetadataAsync();

            Assert.That(await clientExt.IsConnectedAsync());

            client = clientExt.SubstrateClient;
        }

        [Test]
        public async Task QueryHistoricalEventsAsync()
        {
            var eventEncoded = "0x26aa394eea5630e07c48ae0c9558cef780d41e5e16056765bc8461851072c9d7";

            try
            {
                var result = await client.State.GetQueryStorageAsync([Utils.HexToByteArray(eventEncoded)], "0x7bc2f770cbec1afcc0b1490ba25cb4f41fa5674c35ec42b9f1f6b55d270daef8", "0x04a212e756a4c22c947661c5fc4e386accb1cd4c31e95c0c909307ab600870fd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
