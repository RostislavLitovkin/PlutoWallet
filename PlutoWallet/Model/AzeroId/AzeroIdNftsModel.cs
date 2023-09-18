using System;
using Substrate.NetApi;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using Newtonsoft.Json.Linq;
using static Substrate.NetApi.Utils;
using PlutoWallet.Model.AjunaExt;
using Substrate.NetApi.Model.Extrinsics;
using PlutoWallet.Constants;

namespace PlutoWallet.Model.AzeroId
{
	public class AzeroIdNftsModel
	{
        public static async Task<List<NFT>> GetNamesForAddress(string address)
        {
            AjunaClientExt client = new AjunaClientExt(new Uri("wss://ws.test.azero.dev"), ChargeAssetTxPayment.Default());

            await client.ConnectAsync();

            string tld = await AzeroIdModel.GetTld(client);

            string rootKey = "2d010000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(accountId.Encode());

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            var keysPaged = await client.InvokeAsync<JArray>("childstate_getKeys", new object[2] {
                "0x3a6368696c645f73746f726167653a64656661756c743a03d1cf2e15016e7af14df2e656607906e10c891f703a866bb78f6acb8f48f3ff",
                "0x"
            }, CancellationToken.None);

            var unfilteredKeys = keysPaged.Select(p => p.ToString());

            // TO BE CONTINUED ...
            List<NFT> nfts = new List<NFT>();

            foreach (string key in unfilteredKeys)
            {
                if (key.Contains(rootKey) && key.Contains(Utils.Bytes2HexString(accountId.Encode(), HexStringFormat.Pure)))
                {
                    // query the result
                    var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                        "0x3a6368696c645f73746f726167653a64656661756c743a03d1cf2e15016e7af14df2e656607906e10c891f703a866bb78f6acb8f48f3ff",
                        key
                    }, CancellationToken.None);

                    if (temp == null) return null;

                    var result = Utils.HexToByteArray(temp);

                    // decode the bytes to UTF-8 string
                    nfts.Add(new NFT
                    {
                        Name = AzeroIdModel.BytesToString(result) + "." + tld,
                        Image = "azeroiddarkbg.png",
                        Description = "A domain on Aleph Zero's testnet issued by Azero.id",
                        Endpoint = Endpoints.GetEndpointDictionary["azerotestnet"],
                    });
                }
            }

            return nfts;
        }
    }
}

