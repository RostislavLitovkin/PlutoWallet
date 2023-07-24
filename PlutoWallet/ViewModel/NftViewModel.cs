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

        [ObservableProperty]
        private bool isLoading = false;

        public NftViewModel()
		{

        }

		
    }
}

