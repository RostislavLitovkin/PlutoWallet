using Microsoft.VisualStudio.Threading;
using PlutoWallet.Model;
using PlutoWallet.Model.SQLite;
using CollectionKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger);
using UniqueryPlus.Collections;

namespace PlutoWallet.Components.Nft
{
    public partial class OwnedCollectionsListViewModel : BaseListViewModel<CollectionKey, CollectionWrapper>
    {
        public override string Title => "Owned Collections";

        private List<Task<PlutoWalletSubstrateClient>> clientTasks;

        private IAsyncEnumerator<ICollectionBase> uniqueryCollectionEnumerator = null;

        public override async Task LoadMoreAsync(CancellationToken token)
        {
            if (Loading)
            {
                return;
            }

            try
            {
                for (uint i = 0; i < LIMIT; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        break;
                    }

                    if (uniqueryCollectionEnumerator != null && await uniqueryCollectionEnumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        var newCollection = await Model.CollectionModel.ToCollectionWrapperAsync(uniqueryCollectionEnumerator.Current, token);

                        if (newCollection.Key is not null && !ItemsDict.ContainsKey((CollectionKey)newCollection.Key))
                        {
                            ItemsDict.Add((CollectionKey)newCollection.Key, newCollection);
                            await CollectionDatabase.SaveItemAsync(newCollection).ConfigureAwait(false);

                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                Items.Add(newCollection);
                            });
                        }
                    }

                    else
                    {
                        if (clientTasks.Count() == 0)
                        {
                            return;
                        }

                        // Inspiration: https://youtu.be/zhCRX3B7qwY?si=RyNtzmzGHrxO17FD&t=2678
                        var completedClientTask = await Task.WhenAny(clientTasks).WithCancellation(token).ConfigureAwait(false);

                        clientTasks.Remove(completedClientTask);

                        var uniqueryCollectionEnumerable = UniqueryPlus.Collections.CollectionModel.GetCollectionsOwnedByAsync(
                            [(await completedClientTask.ConfigureAwait(false)).SubstrateClient],
                            /*KeysModel.GetSubstrateKey()*/
                            "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                            limit: LIMIT
                        );

                        uniqueryCollectionEnumerator = uniqueryCollectionEnumerable.GetAsyncEnumerator(token);

                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nft owned list error: ");
                Console.WriteLine(ex);
            }
        }

        public override async Task InitialLoadAsync(CancellationToken token)
        {
            Loading = true;

            clientTasks = AjunaClientModel.Clients.Values.Where(_client => true).Select(client => client.Task).ToList();

            await LoadSavedNftsAsync().ConfigureAwait(false);

            await LoadMoreAsync(token).ConfigureAwait(false);

            Loading = false;
        }

        private async Task LoadSavedNftsAsync()
        {
            // Not favourite, owned Nfts
            foreach (var savedNft in await CollectionDatabase.GetCollectionsOwnedByAsync(/*KeysModel.GetSubstrateKey()*/ "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y").ConfigureAwait(false))
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
