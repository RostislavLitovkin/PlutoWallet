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

                Task changeChain = Model.AjunaClientModel.ChangeChainAsync();
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
    }
}

