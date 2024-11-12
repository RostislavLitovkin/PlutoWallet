using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;

namespace PlutoWallet.Components.Nft
{
	public partial class NftGalleryViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<NFT> nfts;

		public NftGalleryViewModel()
		{
			//nfts = new ObservableCollection<NFT>(NftsStorageModel.GetFavouriteNFTs());
        }

		public void Reload(List<NFT> nfts)
		{
            Nfts = new ObservableCollection<NFT>(nfts);
        }
	}
}

