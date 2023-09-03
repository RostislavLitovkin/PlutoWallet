using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using PlutoWallet.Constants;
using System.Numerics;

namespace PlutoWallet.ViewModel
{
	public partial class NftDetailViewModel : ObservableObject
	{
		public NFT Nft { set
            {
                Name = value.Name;
                Description = value.Description;
                Image = value.Image;
                Endpoint = value.Endpoint;
                Attributes = value.Attributes;
            }
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string description;

        [ObservableProperty]
        private string image;

        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private string[] attributes;

        [ObservableProperty]
        private BigInteger collectionId;

        [ObservableProperty]
        private BigInteger itemId;

        [ObservableProperty]
        private Option<string> kodadotUnlockableUrl;

        public NftDetailViewModel()
		{
		}
	}
}

