using System;
using Newtonsoft.Json;
using Substrate.NetApi;
using Uniquery.Types;

namespace PlutoWallet.Model
{
	public class UniqueryModel
	{
		public static async Task<List<NFT>> GetAccountRmrk()
		{
			string address = Utils.GetAddressFrom(KeysModel.GetPublicKeyBytes(), 2);

			List<NftEntity> entities = await Uniquery.Uniquery.GetAccountRmrk(address);
			List<NFT> rmrks = new List<NFT>();

			foreach (NftEntity entity in entities)
			{
                string metadataJson = await Model.IpfsModel.FetchIpfsAsync(entity.Metadata);

                NFT nft = JsonConvert.DeserializeObject<NFT>(metadataJson);

                nft.Image = Model.IpfsModel.ToIpfsLink(nft.Image);

				rmrks.Add(nft);

				Console.WriteLine("rmrk added");
			}

			return rmrks;
        }
	}
}

