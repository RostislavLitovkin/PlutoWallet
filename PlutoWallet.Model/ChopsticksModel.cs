using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace PlutoWallet.Model
{
    internal record class ChopsticksInput
    {
        [JsonPropertyName("endpoint")]
        public string endpoint { get; set; }

        [JsonPropertyName("extrinsic")]
        public string extrinsic { get; set; }
    }

    public class ChopsticksModel
    {
        private const string url = "http://localhost:8000/get-extrinsic-events";

        public static async Task<string> GetExtrinsicEventsAsync(string endpoint, TempUnCheckedExtrinsic extrinsic)
        {
            var httpClient = new HttpClient();

            //var serialized = JsonSerializer.Serialize<ChopsticksInput>(

            using var jsonContent = JsonContent.Create(new ChopsticksInput
            {
                endpoint = "wss://polkadot-rpc.dwellir.com",
                extrinsic = Utils.Bytes2HexString(extrinsic.Encode()).ToString()
            });
            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(
                url,
                jsonContent);
    
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }
    }
}
