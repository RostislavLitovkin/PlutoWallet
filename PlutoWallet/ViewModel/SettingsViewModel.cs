using System;
using CommunityToolkit.Mvvm.ComponentModel;

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

            if(!string.IsNullOrWhiteSpace(url) && !string.IsNullOrWhiteSpace(name)) {
                Preferences.Set("endpointName" + i, name);
                Preferences.Set("endpointUrl" + i, url);
            }
            int testI = i - 1;
            string testEndpointName = "endpointName" + testI;
            string nameTest = Preferences.Get(testEndpointName, "");

        }

        public void ClearEndpoints()
        {
            int i = 1;
            while (Preferences.ContainsKey("endpointName" + i) && Preferences.ContainsKey("endpointUrl" + i)) {
                Preferences.Remove("endpointName" + i);
                Preferences.Remove("endpointUrl" + i);
                i++;
            }
        }
    }
}

