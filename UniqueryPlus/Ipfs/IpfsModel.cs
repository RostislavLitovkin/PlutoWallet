using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace UniqueryPlus.Ipfs
{
    public class IpfsModel
    {
        public static async Task<T?> GetMetadataAsync<T>(string ipfsLink, CancellationToken token)
        {
            var metadataJson = await FetchIpfsAsync(ToIpfsLink(ipfsLink), token);

            return JsonConvert.DeserializeObject<T>(metadataJson);
        }
        public static string ToIpfsLink(string ipfsLink, string ipfsEndpoint = Constants.KODA_IPFS_ENDPOINT)
        {
            if (ipfsLink.Contains("ipfs//"))
            {
                return ipfsEndpoint + ipfsLink.Remove(0, "ipfs//".Length + ipfsLink.IndexOf("ipfs//"));
            }

            if (ipfsLink.Contains("ipfs/"))
            {
                return ipfsEndpoint + ipfsLink.Remove(0, "ipfs/".Length + ipfsLink.IndexOf("ipfs/"));
            }

            if (ipfsLink.Contains("ipfs://"))
            {
                return ipfsEndpoint + ipfsLink.Remove(0, "ipfs://".Length + ipfsLink.IndexOf("ipfs://"));
            }

            if (ipfsLink.Contains("http://") || ipfsLink.Contains("https://"))
            {

                return ipfsLink.Substring(ipfsLink.IndexOf("http"));
            }

            return ipfsEndpoint + RemoveNonHexadecimalCharacters(ipfsLink);
        }

        public static async Task<string> FetchIpfsAsync(string ipfsLink, CancellationToken token)
        {
            HttpClient httpClient = new HttpClient();
            return await httpClient.GetStringAsync(ToIpfsLink(ipfsLink), token);
        }

        public static string RemoveNonHexadecimalCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            Regex regex = new Regex("[^0-9a-zA-Z]", RegexOptions.IgnoreCase);

            if (input.Contains("/"))
            {
                string[] inputSplit = input.Split("/", 2);

                return regex.Replace(inputSplit[0], "") + "/" + inputSplit[1];
            }
            else
            {
                return regex.Replace(input, "");
            }
        }
    }
}
