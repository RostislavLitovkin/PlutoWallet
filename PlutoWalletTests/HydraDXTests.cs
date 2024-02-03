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

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(hdxEndpoint.URLs);

        var client = new SubstrateClientExt(
                    hdxEndpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAsync();

        Assert.That(client.IsConnected);

        await PlutoWallet.Model.HydraDX.Sdk.GetAssets(client, CancellationToken.None);

        Assert.That(PlutoWallet.Model.HydraDX.Sdk.Assets.Any());

        foreach (var asset in PlutoWallet.Model.HydraDX.Sdk.Assets.Keys)
        {
            Console.WriteLine(asset);
        }
    }
}


