using System;
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
            return "https://ipfs.io/ipfs/" + ipfsLink;
        }

		public static async Task<string> FetchIpfsAsync(string ipfsLink)
		{
            HttpClient httpClient = new HttpClient();
			return await httpClient.GetStringAsync(ToIpfsLink(ipfsLink));
        }
    }
}

