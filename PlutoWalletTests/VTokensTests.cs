using System.Numerics;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model.Bifrost;

namespace PlutoWalletTests
{
	public class VTokensTestsss
	{
        [Test]
        public async Task VTokensRedeem()
        {
            Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["bifrost"];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            BigInteger redeemAmount = await VTokenModel.VDotToDot(client, new BigInteger(10000000000), CancellationToken.None);

            Console.WriteLine((double)redeemAmount / 10000000000);
        }
	}
}

