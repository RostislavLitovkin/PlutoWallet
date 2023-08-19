using System;
using System.Text.RegularExpressions;

namespace PlutoWallet.Model
{
	public class IpfsModel
	{
		public static string ToIpfsLink(string ipfsLink)
		{
            if (ipfsLink.Contains("ipfs://ipfs/"))
            {
                return "https://ipfs.io/ipfs/" + ipfsLink.Remove(0, "ipfs://ipfs/".Length + ipfsLink.IndexOf("ipfs://ipfs/"));
            }

            if (ipfsLink.Contains("http://") || ipfsLink.Contains("https://")) {

                return ipfsLink.Substring(ipfsLink.IndexOf("http"));
            }

            return "https://ipfs.io/ipfs/" + RemoveNonHexadecimalCharacters(ipfsLink);
        }

		public static async Task<string> FetchIpfsAsync(string ipfsLink)
		{
            HttpClient httpClient = new HttpClient();
			return await httpClient.GetStringAsync(ToIpfsLink(ipfsLink));
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

