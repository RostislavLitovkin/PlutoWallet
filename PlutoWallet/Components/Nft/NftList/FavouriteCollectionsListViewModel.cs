
using PlutoWallet.Model;
using PlutoWallet.Model.SQLite;
using CollectionKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger);

namespace PlutoWallet.Components.Nft
{
    public partial class FavouriteCollectionsListViewModel : BaseListViewModel<CollectionKey, CollectionWrapper>
    {
        public override string Title => "Favourite Collections";


#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task LoadMoreAsync(CancellationToken token)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            // Nothing, We can not load more than the saved Nfts in the database
        }

        public override async Task InitialLoadAsync(CancellationToken token)
        {
            Loading = true;

            await LoadSavedNftsAsync().ConfigureAwait(false);

            Loading = false;
        }


        private async Task LoadSavedNftsAsync()
        {
            // Not favourite, owned Nfts
            foreach (var savedNft in await CollectionDatabase.GetFavouriteCollectionsAsync().ConfigureAwait(false))
            {
                if (savedNft.Key is not null && !ItemsDict.ContainsKey((CollectionKey)savedNft.Key))
                {
                    ItemsDict.Add((CollectionKey)savedNft.Key, savedNft);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Items.Add(savedNft);
                    });
                }
            }
        }
    }
}