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
            var limit = 25u;

            var clients = AjunaClientModel.Clients.Values.Where(_client => true).Select(async client => await client.Task).Select(t => t.Result.SubstrateClient);

            #region Get Mock Collections
            IEnumerable<ICollectionBase> uniqueryPlusCollections = [
                Model.CollectionModel.GetMockCollection(nftCount: 5000),
                Model.CollectionModel.GetMockCollection(
                    name: "This is a very long name to test how the UI handles potential overflows",
                    nftCount: 5
                ),
                Model.CollectionModel.GetMockCollection(nftCount: 2),
                Model.CollectionModel.GetMockCollection(nftCount: 1),
                Model.CollectionModel.GetMockCollection(nftCount: 0),
            ];

            ObservableCollection<CollectionWrapper> collections = new ObservableCollection<CollectionWrapper>();
            foreach (ICollectionBase collection in uniqueryPlusCollections)
            {
                collections.Add(await Model.CollectionModel.ToCollectionWrapperAsync(collection, CancellationToken.None));
            }
            #endregion
            //var uniqueryCollectionEnumerable = await UniqueryPlus.Collections.CollectionModel.(KeysModel.GetSubstrateKey());


            Collections = collections;

            #region Get Mock Nfts
            IEnumerable<INftBase> uniqueryPlusNfts = [
                Model.NftModel.GetMockNft(),
                Model.NftModel.GetMockNft(
                    name: "This is a very long name to test how the UI handles potential overflows",
                    imageSource: "https://image.w.kodadot.xyz/ipfs/bafybeieo6ghm3gi6n4bqvxhebh2u2celbyp43bf375mzgetua32zujsnoy/?hash=0x2546f88800c47bfe067ff64f0e20d8f0fcfaafbc0e1e5f3df9abd6bf26c009cf"
                )
                ];

            ObservableCollection<NftWrapper> nfts = new ObservableCollection<NftWrapper>();
            foreach (INftBase nft in uniqueryPlusNfts)
            {
                //nfts.Add(Model.NftModel.ToNftWrapper(nft));
            }

            #endregion


            try
            {
                var uniqueryNftEnumerable = UniqueryPlus.Nfts.NftModel.GetNftsOwnedByAsync(clients, KeysModel.GetSubstrateKey(), limit: limit);

                var uniqueryNftEnumerator = uniqueryNftEnumerable.GetAsyncEnumerator();

                for (uint i = 0; i < limit; i++)
                {
                    if (await uniqueryNftEnumerator.MoveNextAsync())
                    {
                        nfts.Add(Model.NftModel.ToNftWrapper(uniqueryNftEnumerator.Current));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Nfts failed here :((");
                Console.WriteLine(ex);
            }
            Nfts = nfts;

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

