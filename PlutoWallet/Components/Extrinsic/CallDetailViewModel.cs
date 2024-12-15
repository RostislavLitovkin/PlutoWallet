using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.Extrinsic
{
    public partial class CallDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private string palletCallName;

        [ObservableProperty]
        private Endpoint endpoint;

        [ObservableProperty]
        private ObservableCollection<EventParameter> callParameters = new ObservableCollection<EventParameter>();

        [ObservableProperty]
        private ObservableCollection<ExtrinsicEvent> extrinsicEvents = new ObservableCollection<ExtrinsicEvent>();
    }
}
