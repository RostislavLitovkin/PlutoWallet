using System;
using System.Text.RegularExpressions;

namespace PlutoWallet.Model
{
	public class IpfsModel
	{
        private const string IPFS_ENDPOINT = "https://image.w.kodadot.xyz/ipfs/";

        public static string ToIpfsLink(string ipfsLink)
		{
            if (ipfsLink.Contains("ipfs://ipfs/"))
            {
                return IPFS_ENDPOINT + ipfsLink.Remove(0, "ipfs://ipfs/".Length + ipfsLink.IndexOf("ipfs://ipfs/"));
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

            string pattern = "[^0-9a-zA-Z]";
            string replacement = "";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.Replace(input, replacement);
        }
    }
}

