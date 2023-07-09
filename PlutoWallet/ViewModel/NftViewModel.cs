using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;

namespace PlutoWallet.ViewModel
{
	public partial class NftViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<NFT> nftMetadata = new ObservableCollection<NFT>();

        public NftViewModel()
		{

        }

		/**
		 * Called in the BasePageViewModel
		 */
		public async Task GetNFTsAsync()
		{

			List<NFT> nfts = new List<NFT>();
			//nfts = (List<NFT>)nfts.Concat<NFT>();

			foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
			{
				if (endpoint.SupportsNfts)
				{
					nfts.AddRange(await NFTsModel.GetNFTsAsync(endpoint));
				}
			}

			try
			{
				nfts.AddRange(await Model.UniqueryModel.GetAccountRmrk());
			}
			catch
			{

			}

			nfts.AddRange(Model.NFTsModel.GetMockNFTs());

			NftMetadata = new ObservableCollection<NFT>(nfts);

		}
	}
}

