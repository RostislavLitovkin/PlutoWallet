using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.NetworkSelect;

namespace PlutoWallet.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string url;

        [ObservableProperty]
        private string name;

        public SettingsViewModel() { 
                        
        }

        public void SaveEndpoint()
        { 
            int i = 1;
             while(Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i ))
                i++;

            if(!string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(Name)) {
                Preferences.Set("endpointName" + i, Name);
                Preferences.Set("endpointUrl" + i, Url);
            }

            Name = "";
            Url = "";

            var networkViewModel = DependencyService.Get<NetworkSelectViewModel>();
            networkViewModel.UpdateNetworks();
        }

        public void ClearEndpoints()
        {
            int i = 1;
            while (Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i)) {
                Preferences.Remove("endpointName" + i);
                Preferences.Remove("endpointUrl" + i);
                i++;
            }

            var networkViewModel = DependencyService.Get<NetworkSelectViewModel>();
            networkViewModel.UpdateNetworks();
        }
    }
}

