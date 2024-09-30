

using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.Events
{
    public partial class EventItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<EventParameter> parametersList = new ObservableCollection<EventParameter>();
    }
}
