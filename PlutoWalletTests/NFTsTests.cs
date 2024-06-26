using System;
using Newtonsoft.Json.Linq;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWalletTests;

public class NFTsTests
{
    string substrateAddress = "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y";

    [Test]
    public async Task PolkadotAssetHubNftsPallet()
    {
        try
        {
            Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["statemint"];

            string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

            var client = new SubstrateClientExt(
                        endpoint,
                        new Uri(bestWebSecket),
                        Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

            await client.ConnectAndLoadMetadataAsync();

            List<NFT> nfts = new List<NFT>();
            
            List<string> collectionItemIds = await NFTsModel.GetNftsAccountAsync(client, substrateAddress, CancellationToken.None);

            foreach (string collectionItemId in collectionItemIds)
            {
                NFT nft = await NFTsModel.GetNftMetadataAsync(client, collectionItemId, CancellationToken.None);
                if (nft != null)
                {
                    NFTsModel.SetNftIds(ref nft, collectionItemId);

                    nft.Endpoint = endpoint;

                    nfts.Add(nft);
                }
            }

            Console.WriteLine(nfts.Count());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    [Test]
    public async Task KusamaAssetHubNftsPallet()
    {
        Endpoint endpoint = PlutoWallet.Constants.Endpoints.GetEndpointDictionary["statemine"];

        string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

        var client = new SubstrateClientExt(
                    endpoint,
                    new Uri(bestWebSecket),
                    Substrate.NetApi.Model.Extrinsics.ChargeTransactionPayment.Default());

        await client.ConnectAndLoadMetadataAsync();


        List<NFT> nfts = new List<NFT>();


        List<string> collectionItemIds = await NFTsModel.GetNftsAccountAsync(client, substrateAddress, CancellationToken.None);

        foreach (string collectionItemId in collectionItemIds)
        {
            NFT nft = await NFTsModel.GetNftMetadataAsync(client, collectionItemId, CancellationToken.None);
            if (nft != null)
            {
                NFTsModel.SetNftIds(ref nft, collectionItemId);

                nft.Endpoint = endpoint;

                nfts.Add(nft);
            }
        }

        Console.WriteLine(nfts.Count());
    }

    [Test]
    public async Task UniqueryNfts()
    {
        var nfts = await UniqueryModel.GetAllNfts(substrateAddress, CancellationToken.None);

        Console.WriteLine(nfts.Count());
    }
}


