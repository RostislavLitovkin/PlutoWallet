using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Numerics;
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
        [JsonPropertyName("fromEndpoint")]
        public required string FromEndpoint { get; set; }

        [JsonPropertyName("extrinsic")]
        public required string Extrinsic { get; set; }

        [JsonPropertyName("blockNumber")]
        public required BigInteger? BlockNumber { get; set; }
    }

    internal record class ChopsticksXcmInput : ChopsticksInput
    {
        

        [JsonPropertyName("toEndpoint")]
        public required string ToEndpoint { get; set; }

        [JsonPropertyName("relay")]
        public required string Relay { get; set; }

        [JsonPropertyName("fromId")]
        public required uint FromId { get; set; }

        [JsonPropertyName("toId")]
        public required uint ToId { get; set; }
    }


    public record class ChopsticksEventsOutput
    {
        [JsonPropertyName("events")]
        public string Events { get; set; }

        [JsonPropertyName("extrinsicIndex")]
        public uint ExtrinsicIndex { get; set; }
    }

    public record class ChopsticksXcmEventsOutput
    {
        [JsonPropertyName("fromEvents")]
        public ChopsticksEventsOutput FromEvents { get; set; }

        [JsonPropertyName("toEvents")]
        public ChopsticksEventsOutput ToEvents { get; set; }
    }

    public class ChopsticksModel
    {
        //private const string url = "https://express-byrr9.ondigitalocean.app";
        private const string url = "http://localhost:8000";

        public static async Task<ChopsticksEventsOutput?> SimulateCallAsync(string endpoint, byte[] extrinsic, BigInteger blockNumber, string senderAddress)
        {
            var httpClient = new HttpClient();

            using var jsonContent = JsonContent.Create(new ChopsticksInput
            {
                FromEndpoint = endpoint,
                Extrinsic = Utils.Bytes2HexString(extrinsic).ToString(),
                BlockNumber = blockNumber,
            });

            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using HttpResponseMessage response = await httpClient.PostAsync(
                    $"{url}/get-extrinsic-events",
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

        public static async Task<ChopsticksXcmEventsOutput?> SimulateXcmCallAsync(string fromEndpoint, string toEndpoint, byte[] extrinsic, string senderAddress)
        {
            var httpClient = new HttpClient();

            using var jsonContent = JsonContent.Create(new ChopsticksXcmInput
            {
                FromEndpoint = fromEndpoint,
                ToEndpoint = toEndpoint,
                Extrinsic = Utils.Bytes2HexString(extrinsic).ToString(),
                Relay = "polkadot",
                FromId = 0,
                ToId = 0,
                BlockNumber = null,
            });

            jsonContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            try
            {
                using HttpResponseMessage response = await httpClient.PostAsync(
                     $"{url}/get-xcm-extrinsic-events",
                    jsonContent);

                var jsonResponse = await response.Content.ReadFromJsonAsync<ChopsticksXcmEventsOutput>();

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
