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
        private const string url = "http://localhost:8000";

        public static async Task<string> GetExtrinsicEventsAsync(string endpoint, TempUnCheckedExtrinsic extrinsic)
        {
            var httpClient = new HttpClient();

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    endpoint,
                    extrinsic = Utils.Bytes2HexString(extrinsic.Encode())
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://localhost:8000",
                jsonContent);
    
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return jsonResponse;
        }
    }
}
