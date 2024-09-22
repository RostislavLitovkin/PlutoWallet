using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWalletTests
{
    internal class TransferTests
    {
        static string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        static Account alice;

        [SetUp]
        public async Task SetupAsync()
        {
            var keyring = new Substrate.NET.Wallet.Keyring.Keyring();

            alice = keyring.AddFromUri("//Alice", default, KeyType.Sr25519).Account;

            Console.WriteLine(alice);
        }

        [Test]
        public async Task NativeTransferAsync()
        {
            Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Local8000];
            Endpoint realEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];


            var client = new SubstrateClientExt(
                realEndpoint,
                        new Uri(endpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());


            var x = await client.ConnectAndLoadMetadataAsync();

            var accountInfo = await AssetsModel.GetNativeBalance(client.SubstrateClient, substrateAddress, CancellationToken.None);

            Console.WriteLine("Free: " + accountInfo.Data.Free.Value);

            Assert.Greater(accountInfo.Data.Free.Value, 0);

            var transfer = TransferModel.NativeTransfer(client, substrateAddress, 10000000000);

            Console.WriteLine();

            #region Temp
            var extrinsic = await client.GetTempUnCheckedExtrinsicAsync(transfer, alice, 64, CancellationToken.None);
            #endregion

            var extrinsicHash = new Hash(HashExtension.Blake2(extrinsic.Encode(), 256));
            string extrinsicHashString = Utils.Bytes2HexString(extrinsicHash);

            Action<string, ExtrinsicStatus> updateExtrinsicsCallback = (string id, ExtrinsicStatus status) =>
            {
                Console.WriteLine("extrinsic state: " + status.ExtrinsicState);

                if (status.ExtrinsicState == ExtrinsicState.Broadcast)
                {
                }
                else if (status.ExtrinsicState == ExtrinsicState.Dropped)
                {
                }

                else if (status.ExtrinsicState == ExtrinsicState.InBlock)
                {
                    Task task = Task.Run(async () =>
                    {
                        var extrinsicDetails = await EventsModel.GetExtrinsicEventsAsync(
                             client,
                             status.Hash,
                             extrinsicHash.Encode()
                        );

                        // temp

                        Console.WriteLine("Events found: " + extrinsicDetails.Events.Count());

                        foreach (var e in extrinsicDetails.Events)
                        {
                            Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                            foreach (var parameter in e.Parameters)
                            {
                                Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                            }
                            Console.WriteLine();
                        }

                        // end temp

                    });
                }

                else if (status.ExtrinsicState == ExtrinsicState.Finalized)
                {
                }

                else
                    Console.WriteLine(status.ExtrinsicState);


                Console.WriteLine("Callback finished");
            };
            var extrinsicId = await client.SubmitExtrinsicAsync(transfer, alice, updateExtrinsicsCallback);

            await Task.Delay(20_000);
        }
    }
}
