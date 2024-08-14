using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;

namespace PlutoWallet.Model
{
    public class ChopsticksModel
    {
        private const string url = "http://localhost:8000/get-extrinsic-events";

        public static async Task<string> GetExtrinsicEventsAsync(string endpoint, TempUnCheckedExtrinsic extrinsic)
        {
            var httpClient = new HttpClient();

            var serialized = JsonSerializer.Serialize(new
            {
                endpoint = "wss://polkadot-rpc.dwellir.com",
                extrinsic = Utils.Bytes2HexString(extrinsic.Encode())
            });

            using StringContent jsonContent = new(
                serialized,
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(
                url,
                jsonContent);
    
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }
    }
}
