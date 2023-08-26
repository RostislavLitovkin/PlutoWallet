using System;
using Newtonsoft.Json;
using PlutoWallet.Constants;

namespace PlutoWallet.Model.Kodadot
{
	public class UnlockablesModel
	{
        /// <summary>
        /// c# implementation of https://github.com/kodadot/nft-gallery/blob/main/services/keywise.ts
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="collectionId"></param>
        /// <returns>URL for Kodadot Unlockable</returns>
        public static async Task<Option<string>> FetchKeywiseAsync(Endpoint endpoint, uint collectionId)
        {
            
            switch (endpoint.Name)
            {
                case ("Statemine"):
                    return await FetchKeywiseAsync("ahk", collectionId);
            }

            return Option<string>.None;
        }

        /// <summary>
        /// c# implementation of https://github.com/kodadot/nft-gallery/blob/main/services/keywise.ts
        /// </summary>
        /// <param name="prefix">Chain prefix, check: https://github.com/kodadot/nft-gallery/blob/main/services/keywise.ts</param>
        /// <param name="collectionId"></param>
        /// <returns>URL for Kodadot Unlockable</returns>
        public static async Task<Option<string>> FetchKeywiseAsync(string prefix, uint collectionId)
        {
            HttpClient httpClient = new HttpClient();
            string json = await httpClient.GetStringAsync("https://keywise.kodadot.workers.dev/resolve/" + prefix + "-" + collectionId);

            var response = JsonConvert.DeserializeObject<UnlockablesResponse>(json);
            if (response.Status != 200)
            {
                return Option<string>.None;
            }
            else
            {
                return Option<string>.Some(response.Url);
            }
        }
    }

    class UnlockablesResponse
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("status")]
        public uint Status { get; set; } = 200;
    }
}

