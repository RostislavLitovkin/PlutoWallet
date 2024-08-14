using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Polkadot.NetApi.Generated.Storage;
using Substrate.NetApi.Generated.Model.pallet_balances.types;
using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Net;
using Polkadot.NetApi.Generated.Model.sp_runtime.multiaddress;
using Polkadot.NetApi.Generated.Model.sp_core.crypto;

namespace PlutoWalletTests
{
    internal class TransferTests
    {
        static string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        static SubstrateClientExt client;

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
            Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Local8000];

            client = new SubstrateClientExt(
                EndpointEnum.Polkadot,
                    hdxEndpoint,
                        new Uri(hdxEndpoint.URLs[0]),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            var x = await client.ConnectAndLoadMetadataAsync();

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
