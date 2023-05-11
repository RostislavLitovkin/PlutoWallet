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
        private const int MAX_BUBBLE_COUNT = 4;

        [ObservableProperty]
        private ObservableCollection<NetworkSelectInfo> networkInfos = new ObservableCollection<NetworkSelectInfo>();

        [ObservableProperty]
        private bool clicked;

        public MultiNetworkSelectViewModel()
        {
            SetupDefault();
        }

        public void SetupDefault()
        {
            int[] defaultNetworks = Endpoints.DefaultNetworks;

            NetworkInfos = new ObservableCollection<NetworkSelectInfo>();
            int[] tempEndpointIndexes = new int[4];

            for (int i = 0; i < MAX_BUBBLE_COUNT; i++)
            {
                int endpointIndex = Preferences.Get("SelectedNetworks" + i, defaultNetworks[i]);
                if (endpointIndex != -1)
                {
                    NetworkInfos.Add(new NetworkSelectInfo
                    {
                        EndpointIndex = endpointIndex,
                        ShowName = i == 0, // true for index 0, otherwise false
                        Name = Endpoints.GetAllEndpoints[endpointIndex].Name,
                        Icon = Endpoints.GetAllEndpoints[endpointIndex].Icon,
                    });
                }
                tempEndpointIndexes[i] = endpointIndex;
            }

            // Update other views
            Task changeChain = Model.AjunaClientModel.ChangeChainGroupAsync(tempEndpointIndexes);
        }
    }

    
}

