using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Model;
using System.Collections.ObjectModel;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft
{
    public partial class NftListOwnedViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<NftWrapper> nfts = new ObservableCollection<NftWrapper>() { };

        private IAsyncEnumerator<INftBase> uniqueryNftEnumerator = null;

        private const uint limit = 4;

        private List<Task<PlutoWalletSubstrateClient>> clientTasks;
        public NftListOwnedViewModel()
        {
            clientTasks = AjunaClientModel.Clients.Values.Where(_client => true).Select(client => client.Task).ToList();

            Task load = LoadMoreNftsAsync();
        }

        [RelayCommand]
        public async Task LoadMoreNftsAsync()
        {
            for (uint i = 0; i < limit; i++)
            {
                Console.WriteLine("Looping over: " + i);
                if (uniqueryNftEnumerator != null && await uniqueryNftEnumerator.MoveNextAsync().ConfigureAwait(false))
                {
                    Console.WriteLine("nftAdded: " + i);

                    Nfts.Add(Model.NftModel.ToNftWrapper(uniqueryNftEnumerator.Current));
                }

                else
                {
                    if (clientTasks.Count() == 0)
                    {
                        return;
                    }

                    Console.WriteLine("Needs to initialize a new enumerator");

                    // Inspiration: https://youtu.be/zhCRX3B7qwY?si=RyNtzmzGHrxO17FD&t=2678
                    var completedClientTask = await Task.WhenAny(clientTasks).ConfigureAwait(false);

                    clientTasks.Remove(completedClientTask);

                    Console.WriteLine("Connecting to clients: " + clientTasks.Count() + "    " + (await completedClientTask.ConfigureAwait(false)).Endpoint.Name);

                    var uniqueryNftEnumerable = UniqueryPlus.Nfts.NftModel.GetNftsOwnedByAsync(
                        [(await completedClientTask.ConfigureAwait(false)).SubstrateClient],
                        /*KeysModel.GetSubstrateKey()*/
                        "5EU6EyEq6RhqYed1gCYyQRVttdy6FC9yAtUUGzPe3gfpFX8y",
                        limit: limit
                    );

                    uniqueryNftEnumerator = uniqueryNftEnumerable.GetAsyncEnumerator();

                    i--;
                }
            }
        }
    }
}
