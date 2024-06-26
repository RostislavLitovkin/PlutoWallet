using System;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;

public class HydraDX
{
    [Test]
    public static async Task GetAssets()
    {
        Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["hydradx"];

        foreach (string url in hdxEndpoint.URLs)
        {
            var client = new SubstrateClientExt(
                        hdxEndpoint,
                        new Uri(url),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            Assert.That(client.IsConnected);

            await PlutoWallet.Model.HydraDX.Sdk.GetAssets(client, CancellationToken.None);

            Assert.That(PlutoWallet.Model.HydraDX.Sdk.Assets.Any());

            foreach (var asset in PlutoWallet.Model.HydraDX.Sdk.Assets.Keys)
            {
                Console.WriteLine(asset);
            }

            Console.WriteLine(PlutoWallet.Model.HydraDX.Sdk.GetSpotPrice("DOT"));
        } 
    }
}


