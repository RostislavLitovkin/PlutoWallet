using System;
using Substrate.NetApi;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;
using Newtonsoft.Json.Linq;
using static Substrate.NetApi.Utils;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Model.AzeroId
{
	public class AzeroIdModel
	{
        public static async Task<string> GetPrimaryNameForAddress(string address)
        {
            var client = Model.AjunaClientModel.Client;

            string rootKey = "0x8f010000";

            /// Actual code logic down here
            List<byte> rootKeyHex = new List<byte>(Utils.HexToByteArray(rootKey));

            var accountId = new AccountId32();
            accountId.Create(Utils.GetPublicKeyFrom(address));

            // concat the rootKey and accountId param
            rootKeyHex.AddRange(accountId.Encode());

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, rootKeyHex.ToArray());

            // query the result
            var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                Constants.AzeroId.TZeroIdPrefixedStorageKey,
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None);
            if (temp == null) return null;

            var result = Utils.HexToByteArray(temp);

            // decode the bytes to UTF-8 string
            return BytesToString(result);
        }

        private static async Task<List<string>> GetNamesForAddress(AjunaClientExt client, string address)
        {
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
                Constants.AzeroId.TZeroIdPrefixedStorageKey,
                "0x"
            }, CancellationToken.None);

            Console.WriteLine("KEYS queried");

            var unfilteredKeys = keysPaged.Select(p => p.ToString());

            // TO BE CONTINUED ...
            List<string> names = new List<string>();

            foreach (string key in unfilteredKeys)
            {
                if (key.Contains(rootKey) && key.Contains(Utils.Bytes2HexString(accountId.Encode(), HexStringFormat.Pure)))
                {
                    // query the result
                    var temp = await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                        Constants.AzeroId.TZeroIdPrefixedStorageKey,
                        key
                    }, CancellationToken.None);

                    if (temp == null) return null;

                    var result = Utils.HexToByteArray(temp);

                    // decode the bytes to UTF-8 string
                    names.Add(BytesToString(result));
                }
            }

            return names;
        }

        public static async Task<string> GetTld()
        {
            var client = Model.AjunaClientModel.Client;

            return await GetTld(client);
        }

        public static async Task<string> GetTld(AjunaClientExt client)
        {
            string rootKey = "0x00000000";

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, Utils.HexToByteArray(rootKey));

            // query the result
            var result = Utils.HexToByteArray(await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                Constants.AzeroId.TZeroIdPrefixedStorageKey,
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None));

            // 2 - 0x (start)
            // 64 - accountId32 (admin)
            // 2/66 - Option<accountId32> (pending_admin)
            // xx - CompactInteger + string (tld)
            // xx - (other)

            // decode the bytes to UTF-8 string
            int index = result[32] == 0 ? 33 : 65;

            return BytesToString(result, ref index);
        }

        public static string BytesToString(byte[] bytesToDecode)
        {
            int index = 0;
            return BytesToString(bytesToDecode, ref index);
        }

        public static string BytesToString(byte[] bytesToDecode, ref int index)
        {
            int tldLength = CompactInteger.Decode(bytesToDecode, ref index);

            byte[] tldBytes = new byte[tldLength];
            Array.Copy(bytesToDecode, index, tldBytes, 0, tldLength);

            index += tldLength;
            return System.Text.Encoding.UTF8.GetString(tldBytes);
        }
    }
}

