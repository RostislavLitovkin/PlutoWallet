using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Substrate.RestClient
{
   public class BaseClient
   {
      public class DefaultResponse
      {
         [JsonProperty("result")]
         public string Result { get; set; }
      }

      protected async Task<T> SendRequestAsync<T>(HttpClient client, string endpoint, string key)
      {
         return await SendRequestAsync<T>(client, $"{endpoint}?key={Uri.EscapeDataString(key.ToLower())}");
      }

      protected async Task<T> SendRequestAsync<T>(HttpClient client, string endpoint)
      {
         var response = await client.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead);
         return await ProcessResponseAsync<T>(response, endpoint);
      }
      private async Task<T> ProcessResponseAsync<T>(HttpResponseMessage response, string endpoint)
      {
         if (response == null || !response.IsSuccessStatusCode)
         {
            throw new InvalidOperationException($"Invalid response received while sending request to {endpoint}!");
         }

         string content = await response.Content.ReadAsStringAsync();
         if (string.IsNullOrEmpty(content))
         {
            throw new InvalidOperationException($"Invalid response data received while sending request to {endpoint}!");
         }

         DefaultResponse json = JsonConvert.DeserializeObject<DefaultResponse>(content);
         var resultingObject = (T)Activator.CreateInstance(typeof(T));
         var resultObjectBaseType = resultingObject as IType;
         resultObjectBaseType.Create(Utils.HexToByteArray(json.Result));

         return resultingObject;
      }
   }
}
