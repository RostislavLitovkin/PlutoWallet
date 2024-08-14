using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PlutoWallet.Model
{
    public class ChopsticksModel
    {
        public static async Task<string> PostAsync()
        {
            var httpClient = new HttpClient();

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(new
                {
                    endpoint = "wss://acala-rpc-2.aca-api.network/ws",
                    extrinsic = "0x1234"
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://localhost:8000",
                jsonContent);
    
            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"{jsonResponse}\n");

            return jsonResponse;

            // Expected output:
            //   POST https://jsonplaceholder.typicode.com/todos HTTP/1.1
            //   {
            //     "userId": 77,
            //     "id": 201,
        }

    }
}
