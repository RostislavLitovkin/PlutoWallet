using System;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;

public class AssetsTests
{
    [Test]
    public async Task AssetsBifrost()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["bifrost"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAsync();

        string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

        Console.WriteLine("Connected");

        var assets = await AssetsModel.GetAssetsMetadataAndAcountNextAsync(client, substrateAddress, 1000, CancellationToken.None);

        Console.WriteLine("Assets: " + assets.Count);
    }

    [Test]
    public async Task TokensBifrost()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["bifrost"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAsync();

        string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

        Console.WriteLine("Connected");

        try
        {
            var tokens = await AssetsModel.GetTokensBalance(client, substrateAddress, CancellationToken.None);

            Console.WriteLine("Tokens: " + tokens.Count);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
