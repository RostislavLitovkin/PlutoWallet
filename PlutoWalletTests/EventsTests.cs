using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model;
using PolkadotAssetHub.NetApi.Generated.Model.asset_hub_polkadot_runtime;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi;
using PlutoWallet.Constants;

namespace PlutoWalletTests
{
    internal class EventsTests
    {
        string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        [Test]
        public async Task GetExtrinsicEventsAsync()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            Hash blockHash = new Hash("0xdc6b77382b2a4e38f6acec43a9ed7f6dfd96563cedcb8110cb6dcef150de6820");

            byte[] extrinsicHash = Utils.HexToByteArray("0xb6cdde5912b61a8e7f687092bbd9537ad3ebc5ab69d2f23239eed97ad38d1b98");

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsAsync(client, blockHash, extrinsicHash);

            Assert.That(extrinsicDetails.Events.Any());

            foreach(var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }
        }

        [Test]
        public async Task GetXcmPalletTransferEventsAsync()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            Hash blockHash = new Hash("0x923ee600b390697dce0e45e08676f0982a03ada57785762bce26aa0cccece051");

            byte[] extrinsicHash = Utils.HexToByteArray("0x24a20fec41ee54fe78719179ab92f1450cb17fda74beea7b525fec7646b4d0e9");

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsAsync(client, blockHash, extrinsicHash);

            Assert.That(extrinsicDetails.Events.Any());

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                Console.WriteLine(e.Parameters.Count() + " parameters found");

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            var currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, extrinsicDetails.Events, endpoint, CancellationToken.None);

        }

        [Test]
        public async Task GetOmnipoolSellEventsAsync()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            Hash blockHash = new Hash("0x467fb6268675b96e707df72d382c14da0045ebc553edd9850414560053870b09");

            byte[] extrinsicHash = Utils.HexToByteArray("0xa1c0f5b91ad540f9fa2ca8143f10e6626a0cf5658e46b690c4b71e010ae0c48b");

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsAsync(client, blockHash, extrinsicHash);

            Assert.That(extrinsicDetails.Events.Any());

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                Console.WriteLine(e.Parameters.Count() + " parameters found");

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            var currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, extrinsicDetails.Events, endpoint, CancellationToken.None);
        }

        [Test]
        public async Task GetTransferEventsAsync()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Opal];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var opalClient = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await opalClient.ConnectAndLoadMetadataAsync();

            Hash blockHash = new Hash("0x2d6fbbcb9505d10e7164d76acd7ecc06ad111436dbdbd1327a9116f180fc6a3e");
            
            byte[] extrinsicHash = Utils.HexToByteArray("0x2087b2359e6a923d723751f685d41ccfdd6ce763eddab4a7473ffd2a727744b9");

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsAsync(opalClient, blockHash, extrinsicHash);

            Assert.That(extrinsicDetails.Events.Any());


            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }

                Console.WriteLine();
            }
        }
    }
}
