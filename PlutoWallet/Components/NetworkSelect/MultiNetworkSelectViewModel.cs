using PlutoWallet.Constants;
using PlutoWallet.ViewModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlutoWallet.Components.Balance;
using System.Collections.ObjectModel;

namespace PlutoWallet.Components.NetworkSelect
{
	public partial class MultiNetworkSelectViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<NetworkSelectEndpoint> networks = new ObservableCollection<NetworkSelectEndpoint>();

        [ObservableProperty]
        private bool clicked;

        public MultiNetworkSelectViewModel()
        {
            SetupDefault();
        }

        public void SetupDefault()
        {
            Clicked = false;

            // Later: Get real endpoint
            var defaultEndpoints = Endpoints.GetAllEndpoints;


            Networks = new ObservableCollection<NetworkSelectEndpoint>();

            foreach (int i in new int[3]{ 0, 2, 3 } ) {
                Networks.Add(new NetworkSelectEndpoint
                {
                    Name = defaultEndpoints[i].Name,
                    Icon = defaultEndpoints[i].Icon,
                });
            }

            Networks[0].ShowName = true;
        }
    }
}

