
using PlutoWallet.Model;
using PlutoWallet.Model.SQLite;
using NftKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger, System.Numerics.BigInteger);

namespace PlutoWallet.Components.Nft
{
    public partial class FavouriteNftsListViewModel : BaseListViewModel<NftKey, NftWrapper>
    {
        public override string Title => "Favourite NFTs";


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
            foreach (var savedNft in await NftDatabase.GetFavouriteNftsAsync().ConfigureAwait(false))
            {
                if (savedNft.Key is not null && !ItemsDict.ContainsKey((NftKey)savedNft.Key))
                {
                    ItemsDict.Add((NftKey)savedNft.Key, savedNft);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Items.Add(savedNft);
                    });
                }
            }
        }
    }
}
