using System;
namespace PlutoWallet.Model
{
	public class IpfsModel
	{
		public static string ToIpfsLink(string ipfsLink)
		{
            Console.WriteLine(ipfsLink);
            Console.WriteLine(ipfsLink.Remove(0, "ipfs://ipfs/".Length + ipfsLink.IndexOf("ipfs://ipfs/")));
            string resultLink = "https://ipfs.io/ipfs/" + ipfsLink.Remove(0, "ipfs://ipfs/".Length + ipfsLink.IndexOf("ipfs://ipfs/"));

			Console.WriteLine(resultLink);
			return resultLink;
		}

		public static async Task<string> FetchIpfsAsync(string ipfsLink)
		{
            HttpClient httpClient = new HttpClient();
			return await httpClient.GetStringAsync(ToIpfsLink(ipfsLink));
        }
	}
}

