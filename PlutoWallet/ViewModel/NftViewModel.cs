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
        private ObservableCollection<NFT> nfts = new ObservableCollection<NFT>() { };

        [ObservableProperty]
        private bool isLoading = false;

        public NftViewModel()
		{

        }

        /**
        * Called in the BasePageViewModel
        */
        public async Task GetNFTsAsync()
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;

            foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
            {
                if (endpoint.SupportsNfts)
                {
                    UpdateNfts(await Model.NFTsModel.GetNFTsAsync(endpoint));
                }
            }

            UpdateNfts(await Model.UniqueryModel.GetAccountRmrk());

            IsLoading = false;
        }

        public void UpdateNfts(List<NFT> newNfts)
        {
            foreach (NFT newNft in newNfts)
            {
                bool isContained = false;
                foreach (NFT savedNft in Nfts)
                {
                    if (savedNft.Equals(newNft))
                    {
                        isContained = true;
                    }
                }

                // if not contained, add the NFT to the layout and saved list
                if (!isContained)
                {
                    Nfts.Add(newNft);
                }
            }
        }

    }
}

