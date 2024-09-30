using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.NetworkSelect
{
    public partial class NetworkSelectEndpoint : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string uRL; // I know, weird looking

        [ObservableProperty]
        private string icon;

        [ObservableProperty]
        private string calamarChainName;

        [ObservableProperty]
        private bool showName = false;
    }
}

