using System;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;

public class AssetsTests
{
    SubstrateClientExt client;

    string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["bifrost"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAsync();
    }

    [Test]
    public async Task BalanceBifrost()
    {
        var accountInfo = await client.SystemStorage.Account(substrateAddress);
        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);
    }

    [Test]
    public async Task AssetsBifrost()
    {
        var assets = await AssetsModel.GetAssetsMetadataAndAcountNextAsync(client, substrateAddress, 1000, CancellationToken.None);

        Console.WriteLine("Assets: " + assets.Count);
    }

    [Test]
    public async Task TokensBifrost()
    {
        var tokens = await AssetsModel.GetBifrostTokensBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Tokens: " + tokens.Count);
    }
}
