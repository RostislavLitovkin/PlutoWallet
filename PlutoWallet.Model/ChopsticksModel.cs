using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Substrate.NetApi.Model.Types;

namespace PlutoWallet.Model
{

    public class ChopsticksMockAccount : Account
    {
        public override Task<byte[]> SignAsync(byte[] _)
        {
            return Task.Run(() => Utils.HexToByteArray("0xdeadbeefcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcd"));
            //                                          
        }
        public override Task<byte[]> SignPayloadAsync(Payload _)
        {
            return Task.Run(() => Utils.HexToByteArray("0xdeadbeefcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcdcd"));
        }
    }
    internal record class ChopsticksInput
    {
        [JsonPropertyName("endpoint")]
        public string Endpoint { get; set; }

        [JsonPropertyName("extrinsic")]
        public string Extrinsic { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }
    }

    public record class ChopsticksEventsOutput
    {
        [JsonPropertyName("events")]
        public string Events { get; set; }

        [JsonPropertyName("extrinsicIndex")]
        public uint ExtrinsicIndex { get; set; }
    }

    public class ChopsticksModel
    {
        private const string url = "https://express-byrr9.ondigitalocean.app/get-extrinsic-events";
        //private const string url = "http://localhost:8000/get-extrinsic-events";

        public static async Task<ChopsticksEventsOutput?> SimulateCallAsync(string endpoint, byte[] extrinsic, string senderAddress)
        {
            var httpClient = new HttpClient();

            using var jsonContent = JsonContent.Create(new ChopsticksInput
            {
                Endpoint = endpoint,
                Extrinsic = Utils.Bytes2HexString(extrinsic).ToString(),
                Address = senderAddress,
            });

            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using HttpResponseMessage response = await httpClient.PostAsync(
                    url,
                    jsonContent);

                var jsonResponse = await response.Content.ReadFromJsonAsync<ChopsticksEventsOutput>();

                return jsonResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
