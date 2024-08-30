using System;
using NUnit.Framework;
using PlutoWallet.Constants;
using PlutoWallet.Model.AjunaExt;
using PlutoWallet.Model.HydraDX;
using PlutoWallet.Model.HydrationModel;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types.Base;

namespace PlutoWalletTests;

public class HydrationTests
{
    static string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    static SubstrateClientExt client;

    [SetUp]
    public async Task SetupAsync()
    {
        Endpoint hdxEndpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary[EndpointEnum.Hydration];

        client = new SubstrateClientExt(
                hdxEndpoint,
                    new Uri(hdxEndpoint.URLs[0]),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();
    }

    [Test]
    public static async Task GetAllAssetsAsync()
    {
        var assets = await PlutoWallet.Model.HydrationModel.HydrationSdk.GetAllAssetsAsync();

        Assert.That(assets.Any());

        foreach (var asset in assets)
        {
            Console.WriteLine(asset.Name + " " + asset.Id);
        }
    }

    [Test]
    public static async Task GetLiquidityMiningDepositAsync()
    {
        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, new Substrate.NetApi.Model.Types.Primitive.U128(5595));
        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, new Substrate.NetApi.Model.Types.Primitive.U128(5592));
        await HydrationLiquidityMiningModel.GetLiquidityMiningDeposit((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, new Substrate.NetApi.Model.Types.Primitive.U128(5593));
    }


    [Test]
    public static async Task GetAssetsAsync()
    {
        await PlutoWallet.Model.HydraDX.Sdk.GetAssets((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, CancellationToken.None);

        Assert.That(PlutoWallet.Model.HydraDX.Sdk.Assets.Any());

        foreach (var asset in PlutoWallet.Model.HydraDX.Sdk.Assets.Keys)
        {
            Console.WriteLine(asset);
        }

        Console.WriteLine(PlutoWallet.Model.HydraDX.Sdk.GetSpotPrice("DOT"));

        Console.WriteLine(PlutoWallet.Model.HydraDX.Sdk.GetSpotPrice(5));

    }

    [Test]
    public static async Task GetDCAPositionsAsync()
    {
        await DCAModel.GetDCAPositions((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, substrateAddress);
    }

    [Test]
    public static async Task GetOmnipoolLiquiditiesAsync()
    {
        var list = await HydrationOmnipoolModel.GetOmnipoolLiquiditiesAsync((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, substrateAddress, CancellationToken.None);

        Assert.That(list.Any());

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i].Amount + "   - " + list[i].Symbol);
            Console.WriteLine(list[i].ToString());
        }
    }

    [Test]
    public static async Task GetOmnipoolLiquidityWithLiquidityMiningAsync()
    {
        string anotherSubstrateAddress = "7J7Kfj7vMsrcRotqZ8BtgFCCSoaVUp21XifM8AYC9Q1SZiax";
        var list = await HydrationLiquidityMiningModel.GetOmnipoolLiquidityWithLiquidityMining((Hydration.NetApi.Generated.SubstrateClientExt)client.SubstrateClient, anotherSubstrateAddress);

        Assert.That(list.Any());

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(list[i].Amount + "   - " + list[i].Symbol);

            foreach (var item in list[i].LiquidityMiningInfos)
            {
                Console.WriteLine("   " + item.RewardAmount + "   - " + item.RewardSymbol);
            }
        }
    }

    [Test]
    public static async Task GetBlockByHash()
    {
        var hash = new Hash("0x8C98202680EC28CC6B92F81ABF152A8A3528A4D2C7BACA569455D24F74476B46");
        var block = await client.SubstrateClient.Chain.GetBlockAsync(hash);
        Assert.That(block.Block.Header.Number.Value == 5883239);
    }
}
