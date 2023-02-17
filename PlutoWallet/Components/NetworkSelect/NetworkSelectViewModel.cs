using PlutoWallet.Constants;
using PlutoWallet.ViewModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlutoWallet.Components.BalanceView;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.NetworkSelect
{
    public partial class NetworkSelectViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Endpoint> networks = new ObservableCollection<Endpoint>();

        private Endpoint selectedEndpoint;

        public NetworkSelectViewModel()
        {
            UpdatePickerItems();

            selectedEndpoint = Networks[0];
        }

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

                Task gettingMetas = customCallsViewModel.GetMetadataAsync();
                Task gettingBalance = balanceViewModel.GetBalanceAsync();
            }
        }

        public void UpdatePickerItems()
        {
            var defaultEndpoints = Endpoints.GetAllEndpoints;
            var customEndpoints = GetEndpointsFromPreferences();

            Networks = new ObservableCollection<Endpoint>(defaultEndpoints.Concat(customEndpoints));
        }

        public void SetupDefaultPickerItems()
        {
            var defaultEndpoints = Endpoints.GetAllEndpoints;

            Networks = new ObservableCollection<Endpoint>(defaultEndpoints);
        }

        private List<Endpoint> GetEndpointsFromPreferences()
        {
            var list = new List<Endpoint>();
            int i = 1;
            while (Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i))
            {
                var newEndpoint = new Endpoint
                {
                    Name = Preferences.Get("endpointName" + i, ""),
                    URL = Preferences.Get("endpointUrl" + i, "")
                };
                list.Add(newEndpoint);
                i++;
            }
            return list;
        }

        // TODO remove method bellow
        /*
        public void UpdateNetworks()
        {
            var tmp = new List<Endpoint>();
            tmp.AddRange(Endpoints.GetAllEndpoints);

            bool isPolkadotInEndpoints = false;
            int i = 1;
            while (Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i))
            {
                tmp.Add(new Endpoint
                {
                    Name = Preferences.Get("endpointName" + i, "") ?? throw new Exception("Endpoint name not found"),
                    URL = Preferences.Get("endpointUrl" + i, "") ?? throw new Exception("Endpoint url not found")
                });
                if (tmp.Last().Name == Preferences.Get("selectedNetworkName", "Polkadot"))
                {
                    SelectedEndpoint = tmp.Last();
                    isPolkadotInEndpoints = true;
                }
                i++;
            }
            networks.Clear();
            for (int j = 0; j < tmp.Count(); ++j)
            {
                networks.Insert(j, tmp[j]);
            }

            if (!isPolkadotInEndpoints)
            {
                SelectedEndpoint = Endpoints.GetAllEndpoints[0];
            }
        }
        */
    }
}

