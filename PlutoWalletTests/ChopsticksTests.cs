using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;

namespace PlutoWalletTests
{
    internal class ChopsticksTests
    {
        static string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

        static string senderAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        [Test]
        public async Task SimulateCallAsync()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot];

            var client = new SubstrateClientExt(
                    endpoint,
                        new Uri(endpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

            var transfer = TransferModel.NativeTransfer(client, substrateAddress, 10000000000);

            var account = new ChopsticksMockAccount();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(senderAddress));

            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(transfer, account, 64, CancellationToken.None, signed: true);

            var header = await client.SubstrateClient.Chain.GetHeaderAsync(null, CancellationToken.None);

            var url = endpoint.URLs[0];

            Console.WriteLine(Utils.Bytes2HexString(extrinsic.Encode()).ToLower());

            var events = await ChopsticksModel.SimulateCallAsync(url, extrinsic.Encode(), header.Number.Value, senderAddress);

            Assert.That(!(events is null));

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: events.ExtrinsicIndex, events.Events, blockNumber: 0, CancellationToken.None);

            Console.WriteLine(extrinsicDetails.Events.Count() + " events found");

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            var currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, extrinsicDetails.Events, endpoint, CancellationToken.None);

            Assert.AreEqual(1, currencyChanges[senderAddress].Values.Count());
            Assert.AreEqual("DOT", currencyChanges[senderAddress].Values.ElementAt(0).Symbol);
            Assert.Greater(-1, currencyChanges[senderAddress].Values.ElementAt(0).Amount);
        }

        [Test]
        public async Task SimulateSwapCallAsync() {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

            var client = new SubstrateClientExt(
                    endpoint,
                        new Uri(endpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

            var swap = new Substrate.NetApi.Model.Extrinsics.Method(Utils.HexToByteArray("0x43")[0], 0, Utils.HexToByteArray("0x0a0000000900000080969800000000000000000000000000a36d794195621f4c07000000000000000802660000000a00000066000000036600000009000000"));

            var account = new ChopsticksMockAccount();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(senderAddress));

            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(swap, account, 64, CancellationToken.None, signed: true);

            var header = await client.SubstrateClient.Chain.GetHeaderAsync(null, CancellationToken.None);

            var url = endpoint.URLs[0];

            Console.WriteLine(Utils.Bytes2HexString(extrinsic.Encode()).ToLower());

            var events = await ChopsticksModel.SimulateCallAsync(url, extrinsic.Encode(), header.Number.Value, senderAddress);

            Assert.That(!(events is null));

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: events.ExtrinsicIndex, events.Events, blockNumber: 0, CancellationToken.None);

            Console.WriteLine(extrinsicDetails.Events.Count() + " events found");

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            var currencyChanges = await TransactionAnalyzerModel.AnalyzeEventsAsync(client, extrinsicDetails.Events, endpoint, CancellationToken.None);

            foreach(var currencyChange in currencyChanges[senderAddress].Values)
            {
                Console.WriteLine(currencyChange.Symbol + " " + currencyChange.Amount);
            }

            Assert.AreEqual(3, currencyChanges[senderAddress].Values.Count());
            //Assert.AreEqual("DOT", currencyChanges[senderAddress].Values.ElementAt(0).Symbol);
            //Assert.Greater(-1, currencyChanges[senderAddress].Values.ElementAt(0).Amount);
        }

        [Test]
        public async Task SimulateXcmCallAsync()
        {
            var fromEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

            var toEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];


            var fromClient = new SubstrateClientExt(
                    fromEndpoint,
                        new Uri(fromEndpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var toClient = new SubstrateClientExt(
                   toEndpoint,
                       new Uri(toEndpoint.URLs[0]),
                       Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await fromClient.ConnectAndLoadMetadataAsync();

            var xa = await toClient.ConnectAndLoadMetadataAsync();


            var transfer = new Substrate.NetApi.Model.Extrinsics.Method(Utils.HexToByteArray("0x89")[0], 1, Utils.HexToByteArray("0x0300010300a10f043205011f00fe4e580003010200a10f01006a4e76d530fa715a95388b889ad33c1665062c3dec9bf0aca3a9e4ff45781e4800"));

            var account = new ChopsticksMockAccount();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(substrateAddress));

            var extrinsic = await fromClient.GetTempUnCheckedExtrinsicAsync(transfer, account, 64, CancellationToken.None, signed: true);

            var header = await fromClient.SubstrateClient.Chain.GetHeaderAsync(null, CancellationToken.None);

            var events = await ChopsticksModel.SimulateXcmCallAsync(fromEndpoint.URLs[0], toEndpoint.URLs[0], extrinsic.Encode());

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(fromClient, extrinsicIndex: events.FromEvents.ExtrinsicIndex, events.FromEvents.Events, blockNumber: 0, CancellationToken.None);

            Console.WriteLine(extrinsicDetails.Events.Count());

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            Console.WriteLine(events.ToEvents.Events.Length);

            var toExtrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(toClient, extrinsicIndex: null, events.ToEvents.Events, blockNumber: 0, CancellationToken.None);

            foreach (var e in toExtrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in e.Parameters)
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

            Assert.That(!(events is null));
        }
    }
}
