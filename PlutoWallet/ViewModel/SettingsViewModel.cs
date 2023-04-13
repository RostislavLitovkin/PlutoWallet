using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.NetworkSelect;
using CommunityToolkit.Mvvm.Input;

namespace PlutoWallet.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string url;

        [ObservableProperty]
        private string name;

        [RelayCommand]
        private void SaveEndpoint()
        { 

        }

        public void ClearEndpoints()
        {
            // Later
        }

        [ObservableProperty]
        private string privateKey = "eck";
        public void ShowPrivateKey()
        {
            privateKey = Preferences.Get("privateKey", "ikjnhdgkfjdng");
        }
    }
}

