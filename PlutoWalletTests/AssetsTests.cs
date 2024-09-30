using System;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi;

namespace PlutoWalletTests;

public class BifrostAssetsTests
{
    SubstrateClient client;

    string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Bifrost];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var clientExt = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await clientExt.ConnectAndLoadMetadataAsync();

        Assert.That(await clientExt.IsConnectedAsync());

        client = clientExt.SubstrateClient;
    }

    [Test]
    public async Task Balance()
    {
        var accountInfo = await AssetsModel.GetNativeBalance(client, substrateAddress, CancellationToken.None);
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
    SubstrateClient client;

    string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var clientExt = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await clientExt.ConnectAndLoadMetadataAsync();

        client = clientExt.SubstrateClient;
    }

    [Test]
    public async Task Balance()
    {
        var accountInfo = await AssetsModel.GetNativeBalance(client, substrateAddress, CancellationToken.None);
        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);
    }

    [Test]
    public async Task Assets()
    {
        var assets = await AssetsModel.GetPolkadotAssetHubAssetsAsync(client, substrateAddress, 1000, CancellationToken.None);

        Console.WriteLine("Assets: " + assets.Count());
    }

    [Test]
    public async Task Tokens()
    {
        var tokens = await AssetsModel.GetHydrationTokensBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Tokens: " + tokens.Count());
    }
}

public class PolkadotAssetHubAssetsTests
{
    SubstrateClient client;

    string substrateAddress = "5CaUEtkTHmVM9aQ6XwiPkKcGscaKKxo5Zy2bCp2sRSXCevRf";

    [SetUp]
    public async Task Setup()
    {
        var endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var clientExt = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await clientExt.ConnectAndLoadMetadataAsync();

        client = clientExt.SubstrateClient;
    }

    [Test]
    public async Task NativeBalanceAsync()
    {
        var accountInfo = await AssetsModel.GetNativeBalance(client, substrateAddress, CancellationToken.None);

        Console.WriteLine("Free: " + accountInfo.Data.Free.Value);

        Assert.Greater(accountInfo.Data.Free.Value, 0);
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
}
