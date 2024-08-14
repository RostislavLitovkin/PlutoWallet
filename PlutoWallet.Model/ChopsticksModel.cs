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
        public string Endpoint { get; set; }

        [JsonPropertyName("extrinsic")]
        public string Extrinsic { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public class ChopsticksModel
    {
        private const string url = "http://localhost:8000/get-extrinsic-events";

        public static async Task<string> SimulateCallAsync(string endpoint, byte[] extrinsic, string senderAddress)
        {
            var httpClient = new HttpClient();

            using var jsonContent = JsonContent.Create(new ChopsticksInput
            {
                Endpoint = endpoint,
                Extrinsic = Utils.Bytes2HexString(extrinsic).ToString(),
                Address = senderAddress,
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
