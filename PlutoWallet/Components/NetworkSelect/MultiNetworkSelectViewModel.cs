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

        /**
         * Also called in the BasePageViewModel
         */
        public void SetupDefault()
        {
            string[] selectedEndpointKeys = Endpoints.GetSelectedEndpointKeys();

            NetworkInfos = new ObservableCollection<NetworkSelectInfo>();

            for(int i = 0; i < selectedEndpointKeys.Length; i++)
            {
                Endpoint endpoint = Endpoints.GetEndpointDictionary[selectedEndpointKeys[i]];
                NetworkInfos.Add(new NetworkSelectInfo
                {
                    EndpointKey = selectedEndpointKeys[i],
                    ShowName = i == 0, // true for the first endpoint, otherwise hidden
                    Name = endpoint.Name,
                    Icon = endpoint.Icon,
                });
            }

            // Update other views
            Task changeChain = Model.AjunaClientModel.ChangeChainGroupAsync(selectedEndpointKeys);
        }


        public void Select(Endpoint selectedEndpoint)
        {
            string[] selectedEndpointKeys = Endpoints.GetSelectedEndpointKeys();

            NetworkInfos = new ObservableCollection<NetworkSelectInfo>();

            for (int i = 0; i < selectedEndpointKeys.Length; i++)
            {
                Endpoint endpoint = Endpoints.GetEndpointDictionary[selectedEndpointKeys[i]];
                NetworkInfos.Add(new NetworkSelectInfo
                {
                    EndpointKey = selectedEndpointKeys[i],
                    ShowName = endpoint.Name == selectedEndpoint.Name,
                    Name = endpoint.Name,
                    Icon = endpoint.Icon,
                });
            }

            Task change = Model.AjunaClientModel.ChangeChainAsync(selectedEndpoint);
        }
    }
}

