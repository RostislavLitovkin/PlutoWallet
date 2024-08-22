using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;

namespace PlutoWalletTests
{
    internal class ChopsticksTests
    {
        static string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        static string senderAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

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

            var url = endpoint.URLs[0];

            Console.WriteLine(Utils.Bytes2HexString(extrinsic.Encode()).ToLower());

            var events = await ChopsticksModel.SimulateCallAsync(url, extrinsic.Encode(), senderAddress);

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

            var currencyChanges = TransactionAnalyzerModel.AnalyzeEvents(extrinsicDetails.Events, endpoint);

            Assert.AreEqual(1, currencyChanges[senderAddress].Values.Count());
            Assert.AreEqual("DOT", currencyChanges[senderAddress].Values.ElementAt(0).Symbol);
            Assert.Greater(-1, currencyChanges[senderAddress].Values.ElementAt(0).Amount);

        }

        [Test]
        public async Task SimulateXcmCallAsync()
        {
            var fromEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

            var toEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];


            var client = new SubstrateClientExt(
                    fromEndpoint,
                        new Uri(fromEndpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

            Console.WriteLine("Connected to chain");

            var transfer = new Substrate.NetApi.Model.Extrinsics.Method(Utils.HexToByteArray("0x89")[0], 0, Utils.HexToByteArray("0x0a00000000e1f50500000000000000000000000004010200419c0100faeb3f91b59c6fa4460a5bd32219f20f404111846ab11fc35c59730e2054523000"));

            var account = new ChopsticksMockAccount();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(senderAddress));

            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(transfer, account, 64, CancellationToken.None, signed: true);

            Console.WriteLine("simulating");
            var events = await ChopsticksModel.SimulateXcmCallAsync(fromEndpoint.URLs[0], toEndpoint.URLs[0], extrinsic.Encode(), senderAddress);

            Console.WriteLine(events);

            Console.WriteLine("simulate completed");

            Assert.That(!(events is null));
        }
    }
}
