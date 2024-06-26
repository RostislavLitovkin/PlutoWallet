using System;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;

public class BifrostAssetsTests
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

        await client.ConnectAndLoadMetadataAsync();
    }

    [Test]
    public async Task Balance()
    {
        var accountInfo = await client.SystemStorage.Account(substrateAddress);
        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);
    }

    [Test]
    public async Task Assets()
    {
        var assets = await AssetsModel.GetPolkadotAssetHubAssetsAsync(client, substrateAddress, 1000, CancellationToken.None);

        Console.WriteLine("Assets: " + assets.Count);
    }

    [Test]
    public async Task Tokens()
    {
        var tokens = await AssetsModel.GetBifrostTokensBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Tokens: " + tokens.Count);
    }
}

public class HydrationAssetsTests
{
    SubstrateClientExt client;

    string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["hydradx"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();
    }

    [Test]
    public async Task Balance()
    {
        var accountInfo = await client.SystemStorage.Account(substrateAddress);
        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);
    }

    [Test]
    public async Task Assets()
    {
        var assets = await AssetsModel.GetPolkadotAssetHubAssetsAsync(client, substrateAddress, 1000, CancellationToken.None);

        Console.WriteLine("Assets: " + assets.Count);
    }

    [Test]
    public async Task Tokens()
    {
        var tokens = await AssetsModel.GetHydrationTokensBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Tokens: " + tokens.Count);
    }
}

public class PolkadotAssetHubAssetsTests
{
    SubstrateClientExt client;

    string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["statemint"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();
    }

    [Test]
    public async Task Balance()
    {
        var accountInfo = await client.SystemStorage.Account(substrateAddress);
        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);
    }

    [Test]
    public async Task Assets()
    {
        try
        {
            var assets = await AssetsModel.GetPolkadotAssetHubAssetsAsync(client, substrateAddress, 1000, CancellationToken.None);

            Console.WriteLine("Assets: " + assets.Count);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    [Test]
    public async Task Tokens()
    {
        var tokens = await AssetsModel.GetTokensBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Tokens: " + tokens.Count);
    }
}
