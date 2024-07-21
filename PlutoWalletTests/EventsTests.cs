using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolkadotAssetHub.NetApi.Generated.Model.asset_hub_polkadot_runtime;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi;
using PlutoWallet.Constants;

namespace PlutoWalletTests
{
    internal class EventsTests
    {
        SubstrateClientExt client;

        string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

        [SetUp]
        public async Task Setup()
        {
            var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();
        }

        [Test]
        public async Task GetExtrinsicEventsAsync()
        {
            Hash blockHash = new Hash("0xdc6b77382b2a4e38f6acec43a9ed7f6dfd96563cedcb8110cb6dcef150de6820");

            byte[] extrinsicHash = Utils.HexToByteArray("0xb6cdde5912b61a8e7f687092bbd9537ad3ebc5ab69d2f23239eed97ad38d1b98");

            var events = await EventsModel.GetExtrinsicEventsAsync<EnumRuntimeEvent>(client, blockHash, extrinsicHash);

            Console.WriteLine(events.Count() + " events found");

            foreach(var e in events)
            {
                Console.WriteLine(e.PalletName + " " + e.EventName + " " + e.Safety);

                foreach (var parameter in EventsModel.GetParametersList(e.Parameters))
                {
                    Console.WriteLine("   +- " + parameter.Name + ": " + parameter.Value);
                }
                Console.WriteLine();
            }
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

            var events = await EventsModel.GetExtrinsicEventsAsync(opalClient, EndpointEnum.Opal, blockHash, extrinsicHash);

            Console.WriteLine(events.Count() + " events found");

            foreach (var e in events)
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
