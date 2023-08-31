using System;
using Substrate.NetApi;
using PlutoWallet.NetApiExt.Generated.Model.sp_core.crypto;

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
                "0x3a6368696c645f73746f726167653a64656661756c743a03d1cf2e15016e7af14df2e656607906e10c891f703a866bb78f6acb8f48f3ff",
                Utils.Bytes2HexString(finalHash)
            }, CancellationToken.None);
            if (temp == null) return null;

            var result = Utils.HexToByteArray(temp);

            // decode the bytes to UTF-8 string
            return BytesToString(result);
        }

        public static async Task<string> GetTld()
        {
            var client = Model.AjunaClientModel.Client;

            string rootKey = "0x00000000";

            // Hash the key
            byte[] finalHash = HashExtension.Hash(Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat, Utils.HexToByteArray(rootKey));

            // query the result
            var result = Utils.HexToByteArray(await client.InvokeAsync<string>("childstate_getStorage", new object[2] {
                "0x3a6368696c645f73746f726167653a64656661756c743a03d1cf2e15016e7af14df2e656607906e10c891f703a866bb78f6acb8f48f3ff",
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

        private static string BytesToString(byte[] bytesToDecode)
        {
            int index = 0;
            return BytesToString(bytesToDecode, ref index);
        }

        private static string BytesToString(byte[] bytesToDecode, ref int index)
        {
            int tldLength = CompactInteger.Decode(bytesToDecode, ref index);

            byte[] tldBytes = new byte[tldLength];
            Array.Copy(bytesToDecode, index, tldBytes, 0, tldLength);

            index += tldLength;
            return System.Text.Encoding.UTF8.GetString(tldBytes);
        }
    }
}

