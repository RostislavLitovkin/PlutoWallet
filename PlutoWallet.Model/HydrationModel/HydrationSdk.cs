using Newtonsoft.Json;
using PlutoWallet.Model.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWallet.Model.HydrationModel
{
    public static class HydrationSdk
    {
        /// <summary>
        /// Source: https://github.com/galacticcouncil/sdk/tree/master/packages/sdk
        /// getAllAssets(): Asset[]
        /// </summary>
        /// <returns></returns>
        public static async Task<List<HydrationAsset>> GetAllAssetsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(PlutoExpress.PLUTO_EXPRESS_API_URL);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var assets = JsonConvert.DeserializeObject<List<HydrationAsset>>(json);
                    return assets ?? [];
                }
                else
                {
                    return [];
                }
            }
        }
    }
}
