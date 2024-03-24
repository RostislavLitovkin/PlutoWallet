using System;
using Newtonsoft.Json;

namespace PlutoWallet.Model
{
	public class VersionModel
	{
		public static async Task<PlutoWalletLatestVersion?> GetPlutoWalletLatestVersionAsync()
		{
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync("https://plutonication.com/plutowallet/latest-version");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<PlutoWalletLatestVersion>();
        }
    }

    public class PlutoWalletLatestVersion
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("versionString")]
        public string VersionString { get; set; }
    }
}

