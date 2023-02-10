
using PlutoWallet.Constants;
using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.ViewModel
{
	public partial class WalletViewModel : ObservableObject
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
            }
        }

        public WalletViewModel()
		{
		}
	}
}

