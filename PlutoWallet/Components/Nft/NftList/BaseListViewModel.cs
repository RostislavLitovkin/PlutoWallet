using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.Nft
{
    public abstract partial class BaseListViewModel<Key, Item> : ObservableObject
    {
        public const uint LIMIT = 4;
        public abstract string Title { get; }

        public Dictionary<Key, Item> ItemsDict = new Dictionary<Key, Item>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoItems))]
        [NotifyPropertyChangedFor(nameof(AnyItems))]
        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        public bool NoItems => !Loading && Items.Count == 0;
        public bool AnyItems => Items.Count() > 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(NoItems))]
        private bool loading = false;

        [RelayCommand]
        public abstract Task LoadMoreAsync(CancellationToken token);
        public abstract Task InitialLoadAsync(CancellationToken token);
    }
}
