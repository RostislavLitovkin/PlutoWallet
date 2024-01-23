using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using static PlutoWallet.Model.NftsStorageModel;

namespace PlutoWallet.Components.Nft
{
	public partial class NftGaleryViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<NFT> nfts;

		public NftGaleryViewModel()
		{
			nfts = new ObservableCollection<NFT>(NftsStorageModel.GetFavouriteNFTs());
        }

		public void Reload(List<NFT> nfts)
		{
            Nfts = new ObservableCollection<NFT>(nfts);
        }
	}
}

