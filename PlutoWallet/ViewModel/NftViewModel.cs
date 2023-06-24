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
			GetNFTsAsync();

        }

		public async Task GetNFTsAsync()
		{
			List<NFT> nfts = new List<NFT>();
			//nfts = (List<NFT>)nfts.Concat<NFT>();

			NftMetadata = new ObservableCollection<NFT>(await Model.NFTsModel.GetNFTsAsync(Endpoints.GetAllEndpoints[11]));
        }
	}
}

