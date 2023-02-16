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
            UpdateNetworks();
        }

        public void UpdateNetworks()
        {
            List<Endpoint> endpointsList = new List<Endpoint>();
            endpointsList.AddRange(Endpoints.GetAllEndpoints);

            int i = 1;
            while (Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i))
            {
                endpointsList.Add(new Endpoint
                {
                    Name = Preferences.Get("endpointName" + i, ""),
                    URL = Preferences.Get("endpointUrl" + i, "")
                });
                i++;
            }

            Networks = endpointsList;

            bool found = false;
            // set the selected network
            foreach (Endpoint endpoint in Networks)
            {
                if (Preferences.Get("selectedNetworkName", "Polkadot") == endpoint.Name)
                {
                    SelectedEndpoint = endpoint;
                    found = true;
                }
            }

            if(!found)
            {
                SelectedEndpoint = Endpoints.GetAllEndpoints[0];
            }
        }
    }
}

