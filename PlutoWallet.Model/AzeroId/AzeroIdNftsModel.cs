using System;
using Substrate.NetApi;
using Substrate.NetApi.Generated.Model.sp_core.crypto;
using Newtonsoft.Json.Linq;
using static Substrate.NetApi.Utils;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Extrinsics;
using PlutoWallet.Constants;
using Newtonsoft.Json;
using AzeroIdResolver;

namespace PlutoWallet.Model.AzeroId
{
	public class AzeroIdNftsModel
	{
        public static async Task<List<NFT>> GetNamesForAddress(string address, CancellationToken token)
        {
            try
            {
                var endpoint = Endpoints.GetEndpointDictionary["azerotestnet"];

                string bestWebSecket = await WebSocketModel.GetFastestWebSocketAsync(endpoint.URLs);

                SubstrateClientExt client = new SubstrateClientExt(endpoint, new Uri(bestWebSecket), ChargeAssetTxPayment.Default());

                await client.ConnectAsync(token);

                string tld = await TzeroId.GetTld(client, token);

                string rootKey = "2d010000";

                /// Actual code logic down here
                List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

                var accountId = new Substrate.NetApi.Generated.Model.sp_core.crypto.AccountId32();
                accountId.Create(Utils.GetPublicKeyFrom(address));

                // concat the rootKey and accountId param
                rootKeyHex.AddRange(accountId.Encode());

                // Hash the key
                byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

                var keysPaged = await client.InvokeAsync<JArray>("childstate_getKeys", new object[2] {
                    PlutoWallet.Constants.AzeroId.TZeroIdPrefixedStorageKey,
                    "0x"
                }, token);

                var unfilteredKeys = keysPaged.Select(p => p.ToString());

                // TO BE CONTINUED ...
                List<NFT> nfts = new List<NFT>();

                foreach (string key in unfilteredKeys)
                {
                    if (key.Contains(rootKey) && key.Contains(Utils.Bytes2HexString(accountId.Encode(), HexStringFormat.Pure)))
                    {
                        // query the result
                        var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                        AzeroIdResolver.Constants.TzeroPrefixedStorageKey,
                        key
                    }, token);

                        if (temp == null) return null;

                        var result = Utils.HexToByteArray(temp);

                        NFT nft = await GetNFTMetadata(AzeroIdResolver.Helpers.BytesToString(result), token);

                        nft.Endpoint = endpoint;

                        nfts.Add(nft);
                    }
                }

                return nfts;
            }
            catch(Exception ex)
            {
                Console.WriteLine("AZERO ID ERROR (in PlutoWallet model): ");
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private async static Task<NFT> GetNFTMetadata(string name, CancellationToken token)
        {
            HttpClient httpClient = new HttpClient();
            string metadataJson = await httpClient.GetStringAsync(new Uri("https://tzero.id/api/v1/metadata/" + name + ".tzero.json"), token);

            Console.WriteLine(metadataJson);
            return JsonConvert.DeserializeObject<AzeroIdNFTWrapper>(metadataJson).Metadata;
        }
    }

    public class AzeroIdNFTWrapper
    {
        [JsonProperty("metadata")]
        public NFT Metadata { get; set; }
    }


}

