using PlutoWallet.Constants;
using PlutoWallet.ViewModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlutoWallet.Components.BalanceView;

namespace PlutoWallet.Components.NetworkSelect
{
    public partial class NetworkSelectViewModel : ObservableObject
    {
        [ObservableProperty]
        private IList<Endpoint> networks;

        private Endpoint selectedEndpoint;

        public Endpoint SelectedEndpoint
        {
            get => selectedEndpoint;
            set
            {
                SetProperty(ref selectedEndpoint, value);

                Preferences.Set("selectedNetworkName", value.Name);
                Preferences.Set("selectedNetwork", value.URL);

                var customCallsViewModel = DependencyService.Get<ViewModel.CustomCallsViewModel>();
                var mainViewModel = DependencyService.Get<MainViewModel>();
                var balanceViewModel = DependencyService.Get<BalanceViewModel>();

                customCallsViewModel.GetMetadataAsync();
                balanceViewModel.GetBalanceAsync();
            }
        }

        public NetworkSelectViewModel()
        {
            networks = Endpoints.GetAllEndpoints;

            // set the selected network
            foreach (Endpoint endpoint in networks)
            {
                if (Preferences.Get("selectedNetworkName", "Polkadot") == endpoint.Name)
                {
                    selectedEndpoint = endpoint;
                }
            }
        }
    }
}

