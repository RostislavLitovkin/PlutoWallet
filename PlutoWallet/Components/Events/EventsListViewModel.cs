using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.Events
{
    public partial class EventsListViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ExtrinsicEvent> extrinsicEvents = new ObservableCollection<ExtrinsicEvent>();
    }
}
