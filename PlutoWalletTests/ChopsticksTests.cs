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

        static string senderAddress = "13QPPJVtxCyJzBdXdqbyYaL3kFxjwVi7FPCxSHNzbmhLS6TR";

        [Test]
        public async Task SimulateCallAsync()
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            var alice = keyring.AddFromUri("//Alice", default, KeyType.Sr25519).Account;

            var hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot];

            var client = new SubstrateClientExt(
                    hdxEndpoint,
                        new Uri(hdxEndpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

            var transfer = TransferModel.NativeTransfer(client, substrateAddress, 10000000000);

            var account = new Account();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(senderAddress));

            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(transfer, alice, 64, CancellationToken.None, signed: true);

            var url = hdxEndpoint.URLs[0];

            Console.WriteLine(Utils.Bytes2HexString(extrinsic.Encode()).ToLower());

            var events = await ChopsticksModel.SimulateCallAsync(url, extrinsic.Encode(), senderAddress);

            var extrinsicDetails = await EventsModel.GetExtrinsicEventsForClientAsync(client, extrinsicIndex: events.ExtrinsicIndex, events.Events, blockNumber: 0, CancellationToken.None);

            Console.WriteLine(extrinsicDetails.Events.Count() + " events found");

            foreach (var e in extrinsicDetails.Events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in EventsModel.GetParametersList(e.Parameters))
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }

        }
    }
}
