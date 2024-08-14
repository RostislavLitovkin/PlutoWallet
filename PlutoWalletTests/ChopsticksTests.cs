using System;
using System.Collections.Generic;
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

            var hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot];

            var client = new SubstrateClientExt(
                    hdxEndpoint,
                        new Uri(hdxEndpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

            var transfer = TransferModel.NativeTransfer(client, substrateAddress, 10000000000);

            var account = new Account();
            account.Create(KeyType.Sr25519, Utils.GetPublicKeyFrom(senderAddress));

            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(transfer, account, 64, CancellationToken.None, signed: false);

            var url = hdxEndpoint.URLs[0];

            var events_string = await ChopsticksModel.SimulateCallAsync(url, transfer.Encode(), senderAddress);

            Assert.IsNotNull(events_string, "Response is null!");
            Assert.That(events_string != "No chopsticks input provided!", "Server responded with code 400!");

            Console.WriteLine(events_string);
        }
    }
}
