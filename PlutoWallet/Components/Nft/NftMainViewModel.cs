using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using System.Collections.ObjectModel;
using UniqueryPlus;
using Microsoft.VisualStudio.Threading;
using PlutoWallet.Model.SQLite;

using NftKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger, System.Numerics.BigInteger);
using CollectionKey = (UniqueryPlus.NftTypeEnum, System.Numerics.BigInteger);
using SubstrateClientExt = PlutoWallet.Model.AjunaExt.SubstrateClientExt;
using UniqueSubstrateClientExt = Unique.NetApi.Generated.SubstrateClientExt;
using System.Numerics;
using CommunityToolkit.Mvvm.Input;

namespace PlutoWallet.Components.Nft
{
    internal partial class NftMainViewModel : ObservableObject
    {
        // There is no ObserableDictionary<_> type
        private Dictionary<NftKey, NftWrapper> featuredNftsDict = new Dictionary<NftKey, NftWrapper>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoFeaturedNfts))]
        [NotifyPropertyChangedFor(nameof(AnyFeaturedNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingFeaturedNfts))]
        private ObservableCollection<NftWrapper> featuredNfts = new ObservableCollection<NftWrapper>();
        public bool NoFeaturedNfts => !Loading && FeaturedNfts.Count() == 0;
        public bool AnyFeaturedNfts => FeaturedNfts.Count() > 0;
        public bool LoadingFeaturedNfts => Loading && FeaturedNfts.Count() < 3;

        [ObservableProperty]
        private bool featuredNftErrorIsVisible = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoOwnedCollections))]
        [NotifyPropertyChangedFor(nameof(NoOwnedNfts))]
        [NotifyPropertyChangedFor(nameof(NoFeaturedNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingOwnedCollections))]
        [NotifyPropertyChangedFor(nameof(LoadingOwnedNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingFeaturedNfts))]
        private bool loading = false;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoFavouriteCollections))]
        [NotifyPropertyChangedFor(nameof(NoFavouriteNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingFavouriteCollections))]
        [NotifyPropertyChangedFor(nameof(LoadingFavouriteNfts))]
        private bool databaseLoading = true;

        // There is no ObserableDictionary<_> type
        private Dictionary<NftKey, NftWrapper> favouriteNftsDict = new Dictionary<NftKey, NftWrapper>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoFavouriteNfts))]
        [NotifyPropertyChangedFor(nameof(AnyFavouriteNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingFavouriteNfts))]
        private ObservableCollection<NftWrapper> favouriteNfts = new ObservableCollection<NftWrapper>();
        public bool NoFavouriteNfts => !DatabaseLoading && FavouriteNfts.Count() == 0;
        public bool AnyFavouriteNfts => FavouriteNfts.Count() > 0;
        public bool LoadingFavouriteNfts => DatabaseLoading && FavouriteNfts.Count() < 5;

        // There is no ObserableDictionary<_> type
        private Dictionary<NftKey, NftWrapper> ownedNftsDict = new Dictionary<NftKey, NftWrapper>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoOwnedNfts))]
        [NotifyPropertyChangedFor(nameof(AnyOwnedNfts))]
        [NotifyPropertyChangedFor(nameof(LoadingOwnedNfts))]
        private ObservableCollection<NftWrapper> ownedNfts = new ObservableCollection<NftWrapper>();
        public bool NoOwnedNfts => !Loading && OwnedNfts.Count == 0;
        public bool AnyOwnedNfts => OwnedNfts.Count() > 0;
        public bool LoadingOwnedNfts => Loading && OwnedNfts.Count() < 5;

        // There is no ObserableDictionary<_> type
        private Dictionary<CollectionKey, CollectionWrapper> ownedCollectionsDict = new Dictionary<CollectionKey, CollectionWrapper>();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoOwnedCollections))]
        [NotifyPropertyChangedFor(nameof(AnyOwnedCollections))]
        [NotifyPropertyChangedFor(nameof(LoadingOwnedCollections))]
        private ObservableCollection<CollectionWrapper> ownedCollections = new ObservableCollection<CollectionWrapper>();
        public bool NoOwnedCollections => !Loading && OwnedCollections.Count == 0;
        public bool AnyOwnedCollections => OwnedCollections.Count() > 0;
        public bool LoadingOwnedCollections => Loading && OwnedCollections.Count() < 5;

        // There is no ObserableDictionary<_> type
        private Dictionary<CollectionKey, CollectionWrapper> favouriteCollectionsDict = new Dictionary<CollectionKey, CollectionWrapper>();
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoFavouriteCollections))]
        [NotifyPropertyChangedFor(nameof(AnyFavouriteCollections))]
        [NotifyPropertyChangedFor(nameof(LoadingFavouriteCollections))]
        private ObservableCollection<CollectionWrapper> favouriteCollections = new ObservableCollection<CollectionWrapper>();
        public bool NoFavouriteCollections => !DatabaseLoading && FavouriteCollections.Count == 0;
        public bool AnyFavouriteCollections => FavouriteCollections.Count() > 0;
        public bool LoadingFavouriteCollections => DatabaseLoading && FavouriteCollections.Count() < 5;


        private const uint limit = 100;
        private const uint displayLimit = 5;

        private List<Task<PlutoWalletSubstrateClient>> clientTasks;
        public NftMainViewModel()
        {

        }

        public async Task ConnectClientsAsync(CancellationToken token)
        {
            if (Loading)
            {
                return;
            }

            Loading = true;
            DatabaseLoading = true;

            try
            {
                await LoadSavedNftsAsync().ConfigureAwait(false);
                await LoadSavedCollectionsAsync().ConfigureAwait(false);

                DatabaseLoading = false;

                clientTasks = AjunaClientModel.Clients.Values.Where(_client => true).Select(client => client.Task).ToList();

                while (clientTasks.Count() > 0)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }

                    // Inspiration: https://youtu.be/zhCRX3B7qwY?si=RyNtzmzGHrxO17FD&t=2678
                    var completedClientTask = await Task.WhenAny(clientTasks).WithCancellation(token).ConfigureAwait(false);

                    clientTasks.Remove(completedClientTask);

                    var client = await completedClientTask.ConfigureAwait(false);
                    await GetFeaturedNftsAsync(client, token);

                    await GetOwnedNftsAsync(client, token);

                    await GetOwnedCollectionsAsync(client, token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nft error: ");
                Console.WriteLine(ex);
            }

            Loading = false;
        }
        private async Task GetFeaturedNftsAsync(SubstrateClientExt client, CancellationToken token)
        {
            try
            {
                switch (client.SubstrateClient)
                {
                    case PolkadotAssetHub.NetApi.Generated.SubstrateClientExt:

                        // This would be connected to an API

                        (uint,uint)[] ahpIds = [(208, 1), (258, 4230904976)];

                        foreach (var id in ahpIds)
                        {

                            var newNft = Model.NftModel.ToNftWrapper(await UniqueryPlus.Nfts.NftModel.GetNftByIdAsync(client.SubstrateClient, NftTypeEnum.PolkadotAssetHub_NftsPallet, id.Item1, id.Item2, token).ConfigureAwait(false));

                            if (newNft.Key is not null && !featuredNftsDict.ContainsKey((NftKey)newNft.Key))
                            {
                                featuredNftsDict.Add((NftKey)newNft.Key, newNft);
                            }
                            else
                            {
                                Console.WriteLine("Not added");
                                // Was already loaded
                            }
                        }

                        break;
                    case Mythos.NetApi.Generated.SubstrateClientExt:
                        (BigInteger, BigInteger)[] mythosIds = [(BigInteger.Parse("86219270927352332455509372315086258213278212512"), new BigInteger(101))];

                        foreach (var id in mythosIds)
                        {
                            var newNft = Model.NftModel.ToNftWrapper(await UniqueryPlus.Nfts.NftModel.GetNftByIdAsync(client.SubstrateClient, NftTypeEnum.Mythos, id.Item1, id.Item2, token).ConfigureAwait(false));

                            if (newNft.Key is not null && !featuredNftsDict.ContainsKey((NftKey)newNft.Key))
                            {
                                featuredNftsDict.Add((NftKey)newNft.Key, newNft);
                            }
                            else
                            {
                                Console.WriteLine("Not added");
                                // Was already loaded
                            }
                        }

                        break;
                    case UniqueSubstrateClientExt:
                        (BigInteger, BigInteger)[] uniqueIds = [(new BigInteger(304), new BigInteger(1))];

                        foreach (var id in uniqueIds)
                        {
                            var newNft = Model.NftModel.ToNftWrapper(await UniqueryPlus.Nfts.NftModel.GetNftByIdAsync(client.SubstrateClient, NftTypeEnum.Unique, id.Item1, id.Item2, token).ConfigureAwait(false));

                            if (newNft.Key is not null && !featuredNftsDict.ContainsKey((NftKey)newNft.Key))
                            {
                                featuredNftsDict.Add((NftKey)newNft.Key, newNft);
                            }
                            else
                            {
                                Console.WriteLine("Not added");
                                // Was already loaded
                            }
                        }

                        break;

                }

                FeaturedNfts = new ObservableCollection<NftWrapper>(featuredNftsDict.Values);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Featured nfts error:");
                Console.WriteLine(ex);

                FeaturedNftErrorIsVisible = true;
            }
        }

        private async Task GetOwnedNftsAsync(SubstrateClientExt client, CancellationToken token)
        {
            var uniqueryNftEnumerable = UniqueryPlus.Nfts.NftModel.GetNftsOwnedByAsync(
                        [client.SubstrateClient],
                        /*KeysModel.GetSubstrateKey()*/
                        "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                        limit: limit
                    );

            var uniqueryNftEnumerator = uniqueryNftEnumerable.GetAsyncEnumerator(token);

            while (uniqueryNftEnumerator != null && await uniqueryNftEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                var newNft = Model.NftModel.ToNftWrapper(uniqueryNftEnumerator.Current);

                if (newNft.Key is not null && !ownedNftsDict.ContainsKey((NftKey)newNft.Key) && !favouriteNftsDict.ContainsKey((NftKey)newNft.Key))
                {
                    ownedNftsDict.Add((NftKey)newNft.Key, newNft);
                    await NftDatabase.SaveItemAsync(newNft).ConfigureAwait(false);

                    if (ownedNftsDict.Count() <= displayLimit)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (OwnedNfts.Count() < displayLimit)
                                OwnedNfts.Add(newNft);
                        });
                    }
                }
                else
                {
                    // Was already loaded
                }
            }
        }

        private async Task GetOwnedCollectionsAsync(SubstrateClientExt client, CancellationToken token)
        {
            var uniqueryCollectionEnumerable = UniqueryPlus.Collections.CollectionModel.GetCollectionsOwnedByAsync(
                [client.SubstrateClient],
                /*KeysModel.GetSubstrateKey()*/
                "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                limit: limit
            );

            var uniqueryCollectionEnumerator = uniqueryCollectionEnumerable.GetAsyncEnumerator(token);

            while (uniqueryCollectionEnumerator != null && await uniqueryCollectionEnumerator.MoveNextAsync().ConfigureAwait(false))
            {
                var newCollection = await Model.CollectionModel.ToCollectionWrapperAsync(uniqueryCollectionEnumerator.Current, token).ConfigureAwait(false);

                if (newCollection.Key is not null && !ownedCollectionsDict.ContainsKey((CollectionKey)newCollection.Key) && !favouriteCollectionsDict.ContainsKey((CollectionKey)newCollection.Key))
                {
                    ownedCollectionsDict.Add((CollectionKey)newCollection.Key, newCollection);
                    await CollectionDatabase.SaveItemAsync(newCollection).ConfigureAwait(false);

                    if (ownedCollectionsDict.Count() <= displayLimit)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (OwnedCollections.Count() < displayLimit)
                                OwnedCollections.Add(newCollection);
                        });
                    }
                }
                else
                {
                    // Was already loaded
                }
            }

            OwnedCollections = new ObservableCollection<CollectionWrapper>(ownedCollectionsDict.Values);
        }

        private async Task LoadSavedNftsAsync()
        {
            try
            {
                // Favourite Nfts
                foreach (var savedNft in await NftDatabase.GetFavouriteNftsAsync().ConfigureAwait(false))
                {
                    if (savedNft.Key is not null && !favouriteNftsDict.ContainsKey((NftKey)savedNft.Key))
                    {
                        favouriteNftsDict.Add((NftKey)savedNft.Key, savedNft);

                        if (favouriteNftsDict.Count() <= displayLimit)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (FavouriteNfts.Count() < displayLimit)
                                    FavouriteNfts.Add(savedNft);
                            });
                        }
                    }
                }

                // Not favourite, owned Nfts
                foreach (var savedNft in await NftDatabase.GetNotFavouriteNftsOwnedByAsync(/*KeysModel.GetSubstrateKey()*/ "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y").ConfigureAwait(false))
                {
                    if (savedNft.Key is not null && !ownedNftsDict.ContainsKey((NftKey)savedNft.Key))
                    {
                        ownedNftsDict.Add((NftKey)savedNft.Key, savedNft);

                        if (ownedNftsDict.Count() <= displayLimit)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                if (OwnedNfts.Count() < displayLimit)
                                    OwnedNfts.Add(savedNft);
                            });
                        }
                        Console.WriteLine("Loaded saved nft: " + savedNft.NftBase.Type + " " + savedNft.NftBase.CollectionId + " - " + savedNft.NftBase.Id);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task LoadSavedCollectionsAsync()
        {
            // Favourite Collections
            foreach (var savedCollection in await CollectionDatabase.GetFavouriteCollectionsAsync().ConfigureAwait(false))
            {
                if (savedCollection.Key is not null && !favouriteCollectionsDict.ContainsKey((CollectionKey)savedCollection.Key))
                {
                    favouriteCollectionsDict.Add((CollectionKey)savedCollection.Key, savedCollection);

                    if (favouriteCollectionsDict.Count() <= displayLimit)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (FavouriteCollections.Count() < displayLimit)
                                FavouriteCollections.Add(savedCollection);
                        });
                    }
                }
            }

            // Not favourite, owned Collections
            foreach (var savedCollection in await CollectionDatabase.GetNotFavouriteCollectionsOwnedByAsync(/*KeysModel.GetSubstrateKey()*/ "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y").ConfigureAwait(false))
            {
                if (savedCollection.Key is not null && !ownedCollectionsDict.ContainsKey((CollectionKey)savedCollection.Key))
                {
                    ownedCollectionsDict.Add((CollectionKey)savedCollection.Key, savedCollection);

                    if (ownedCollectionsDict.Count() <= displayLimit)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            if (OwnedCollections.Count() < displayLimit)
                                OwnedCollections.Add(savedCollection);
                        });
                    }
                }
            }
        }

        [RelayCommand]
        public Task ShowAllOwnedNftsAsync() => Application.Current.MainPage.Navigation.PushAsync(new NftListPage(new OwnedNftsListViewModel()));

        [RelayCommand]
        public Task ShowAllFavouriteNftsAsync() => Application.Current.MainPage.Navigation.PushAsync(new NftListPage(new FavouriteNftsListViewModel()));

        [RelayCommand]
        public Task ShowAllOwnedCollectionsAsync() => Application.Current.MainPage.Navigation.PushAsync(new CollectionListPage(new OwnedCollectionsListViewModel()));

        [RelayCommand]
        public Task ShowAllFavouriteCollectionsAsync() => Application.Current.MainPage.Navigation.PushAsync(new CollectionListPage(new FavouriteCollectionsListViewModel()));
    }
}
