using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Substrate.ServiceLayer.Model;
using System;
using Substrate.NetApi.Model.Types.Base;

namespace Substrate.RestClient.Mockup
{
   public class MockupBaseClient
   {
      protected async Task<bool> SendMockupRequestAsync(HttpClient client, string storage, byte[] value, string key)
      {
         var request = new MockupRequest()
         {
            Storage = storage,
            Value = value,
            Key = key
         };

         string content = JsonConvert.SerializeObject(request);
         byte[] buffer = Encoding.UTF8.GetBytes(content);
         var byteContent = new ByteArrayContent(buffer);
         byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
         HttpResponseMessage response = await client.PostAsync("mockup/data", byteContent);

         return await ProcessResponseAsync(response);
      }

      protected async Task<bool> SendMockupRequestAsync(HttpClient client, string storage, byte[] value)
      {
         return await SendMockupRequestAsync(client, storage, value, string.Empty);
      }

      private async Task<bool> ProcessResponseAsync(HttpResponseMessage response)
      {
         if (response == null || !response.IsSuccessStatusCode)
         {
            throw new InvalidOperationException($"Invalid response received while sending request to mockup endpoint!");
         }
         string result = await response.Content.ReadAsStringAsync();
         return JsonConvert.DeserializeObject<bool>(result);
      }
   }
}
