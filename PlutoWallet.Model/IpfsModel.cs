using System;
using System.Text.RegularExpressions;

namespace PlutoWallet.Model
{
	public class IpfsModel
	{
        private const string IPFS_ENDPOINT = "https://image.w.kodadot.xyz/ipfs/";

        public static string ToIpfsLink(string ipfsLink)
		{
            if (ipfsLink.Contains("ipfs//"))
            {
                return IPFS_ENDPOINT + ipfsLink.Remove(0, "ipfs//".Length + ipfsLink.IndexOf("ipfs//"));
            }

            if (ipfsLink.Contains("ipfs/"))
            {
                return IPFS_ENDPOINT + ipfsLink.Remove(0, "ipfs/".Length + ipfsLink.IndexOf("ipfs/"));
            }

            if (ipfsLink.Contains("ipfs://"))
            {
                return IPFS_ENDPOINT + ipfsLink.Remove(0, "ipfs://".Length + ipfsLink.IndexOf("ipfs://"));
            }

            if (ipfsLink.Contains("http://") || ipfsLink.Contains("https://")) {

                return ipfsLink.Substring(ipfsLink.IndexOf("http"));
            }

            return IPFS_ENDPOINT + RemoveNonHexadecimalCharacters(ipfsLink);
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

                return regex.Replace(inputSplit[0], "")  + "/" + inputSplit[1];
            }
            else
            {
                return regex.Replace(input, "");
            }
        }
    }
}

