using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using PlutoWallet.Components.Nft;
using UniqueryPlus.Collections;
using UniqueryPlus.Nfts;

namespace PlutoWallet.ViewModel
{
	public partial class NftViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<NftWrapper> nfts = new ObservableCollection<NftWrapper>() { };

        [ObservableProperty]
        private ObservableCollection<CollectionWrapper> collections = new ObservableCollection<CollectionWrapper>() { };

        [ObservableProperty]
        private bool noNftsIsVisible = false;

        public NftViewModel()
		{

        }


        /**
        * Called in the BasePageViewModel
        */
        public async Task GetNFTsAsync(string substrateAddress, CancellationToken token)
        {
            #region Get Mock Collections
            IEnumerable<ICollectionBase> uniqueryPlusCollections = [
                Model.CollectionModel.GetMockCollection(5000),
                Model.CollectionModel.GetMockCollection(5),
                Model.CollectionModel.GetMockCollection(2),
                Model.CollectionModel.GetMockCollection(1),
                Model.CollectionModel.GetMockCollection(0),
            ];

            ObservableCollection<CollectionWrapper> collections = new ObservableCollection<CollectionWrapper>();
            foreach (ICollectionBase collection in uniqueryPlusCollections)
            {
                collections.Add(await Model.CollectionModel.ToCollectionWrapperAsync(collection, CancellationToken.None));
            }

            Collections = collections;
            #endregion

            #region Get Mock Nfts
            IEnumerable<INftBase> uniqueryPlusNfts = [
                Model.NftModel.GetMockNft(),
                Model.NftModel.GetMockNft()
                ];

            ObservableCollection<NftWrapper> nfts = new ObservableCollection<NftWrapper>();
            foreach (INftBase nft in uniqueryPlusNfts)
            {
                nfts.Add(Model.NftModel.ToNftWrapper(nft));
            }

            Nfts = nfts;

            #endregion


            #region Get Nfts
            /*
            var nftLoadingViewModel = DependencyService.Get<NftLoadingViewModel>();

            nftLoadingViewModel.IsVisible = true;

            UpdateNfts(NftsStorageModel.GetFavouriteNFTs());

            foreach (Endpoint endpoint in Endpoints.GetAllEndpoints)
            {
                if (endpoint.SupportsNfts)
                {
                    UpdateNfts(await Model.NFTsModel.GetNFTsAsync(substrateAddress, endpoint, token));
                }
            }


            UpdateNfts(await Model.UniqueryModel.GetAllNfts(substrateAddress, token));

            // Broken, not a priority rn to fix
            //UpdateNfts(await Model.AzeroId.AzeroIdNftsModel.GetNamesForAddress(Model.KeysModel.GetSubstrateKey(), token));

            nftLoadingViewModel.IsVisible = false;

            NoNftsIsVisible = Nfts.Count() == 0;
            */
            #endregion
        }

        public void UpdateNfts(List<NFT> newNfts)
        {
            /*
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
            }*/
        }

    }
}

