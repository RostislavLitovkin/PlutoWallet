using System;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model.HydraDX;
using PlutoWallet.Model.HydrationModel;
using Substrate.NetApi;

namespace PlutoWalletTests;

public class HydrationTests
{
    static string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [Test]
    public static async Task GetAllAssetsAsync()
    {
        var assets = await PlutoWallet.Model.HydrationModel.HydrationSdk.GetAllAssetsAsync();

        Assert.That(assets.Any());

        foreach(var asset in assets)
        {
            Console.WriteLine(asset.Name + " " + asset.Id);
        }
    }

    [Test]
    public static async Task GetLiquidityMiningDepositAsync()
    {
        Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["hydradx"];

        var client = new Hydration.NetApi.Generated.SubstrateClientExt(
                    new Uri(hdxEndpoint.URLs[0]),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAsync();


        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit(client, 5595);
        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit(client, 5592);
        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit(client, 5593);

    }


    [Test]
    public static async Task GetAssetsAsync()
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

    [Test]
    public static async Task GetDCAPositionsAsync()
    {
        Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["hydradx"];

        var client = new SubstrateClientExt(
                    hdxEndpoint,
                    new Uri(hdxEndpoint.URLs[0]),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();

        await DCAModel.GetDCAPositions(client, substrateAddress);
    }

    [Test]
    public static async Task GetOmnipoolLiquidityAmountAsync()
    {
        Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["hydradx"];

        var client = new SubstrateClientExt(
                    hdxEndpoint,
                    new Uri(hdxEndpoint.URLs[0]),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();

        var list = await OmnipoolModel.GetOmnipoolLiquidityAmount(client, substrateAddress);

        Assert.That(list.Any());

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i].Amount + "   - " + list[i].Symbol);
            Console.WriteLine(list[i].ToString());
        }
    }
}


