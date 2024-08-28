using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;

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
        public async Task PolkadotNativeTransferAsync()
        {
            Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Local8000];

            var client = new SubstrateClientExt(
                EndpointEnum.Polkadot,
                    endpoint,
                        new Uri(endpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());


            var x = await client.ConnectAndLoadMetadataAsync();

            var accountInfo = await AssetsModel.GetNativeBalance(client.SubstrateClient, substrateAddress, CancellationToken.None);

            Console.WriteLine("Free: " + accountInfo.Data.Free.Value);

            Assert.Greater(accountInfo.Data.Free.Value, 0);

            var transfer = TransferModel.NativeTransfer(client, substrateAddress, 10000000000);

            Console.WriteLine();

            Action<string, ExtrinsicStatus> callback = async (string id, ExtrinsicStatus status) =>
            {
                Console.WriteLine(status.ExtrinsicState);
            };

            var extrinsicId = await client.SubmitExtrinsicAsync(transfer, alice, callback);

            await Task.Delay(20_000);
        }

    }
}
