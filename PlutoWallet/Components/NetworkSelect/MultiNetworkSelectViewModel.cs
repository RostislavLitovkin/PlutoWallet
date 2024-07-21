using PlutoWallet.Constants;
using PlutoWallet.ViewModel;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlutoWallet.Components.Balance;
using System.Collections.ObjectModel;
using PlutoWallet.Model;

namespace PlutoWallet.Components.NetworkSelect
{
    public partial class MultiNetworkSelectViewModel : ObservableObject
	{
        [ObservableProperty]
        private ObservableCollection<NetworkSelectInfo> networkInfos = new ObservableCollection<NetworkSelectInfo>();

        [ObservableProperty]
        private bool clicked;

        public MultiNetworkSelectViewModel()
        {
            Console.WriteLine("MultiNetworkSelectViewMode Constructor");
        }

        /// <summary>
        /// Also called in the BasePageViewModel
        /// </summary>
        public void SetupDefault()
        {
            var selectedEndpointKeys = EndpointsModel.GetSelectedEndpointKeys().ToArray();

            var networkInfosList = new List<NetworkSelectInfo>();

            for(int i = 0; i < selectedEndpointKeys.Length; i++)
            {
                Endpoint endpoint = EndpointsModel.GetEndpoint(selectedEndpointKeys[i]);
                networkInfosList.Add(new NetworkSelectInfo
                {
                    EndpointKey = selectedEndpointKeys[i],
                    ShowName = i == 0, // true for the first endpoint, otherwise hidden
                    Name = endpoint.Name,
                    Icon = endpoint.Icon,
                    DarkIcon = endpoint.DarkIcon,
                    EndpointConnectionStatus = EndpointConnectionStatus.Loading,
                });
            }

            NetworkInfos = new ObservableCollection<NetworkSelectInfo>(networkInfosList);

            // Update other views
            Task changeChain = Model.AjunaClientModel.ChangeChainGroupAsync(selectedEndpointKeys);
        }

        public void Select(Endpoint selectedEndpoint)
        {
            var selectedEndpointKeys = EndpointsModel.GetSelectedEndpointKeys().ToArray();

            for (int i = 0; i < selectedEndpointKeys.Length; i++)
            {
                Endpoint endpoint = EndpointsModel.GetEndpoint(selectedEndpointKeys[i]);
                NetworkInfos[i].ShowName = endpoint.Name == selectedEndpoint.Name;
            }

            UpdateNetworkInfos();


            Task change = Model.AjunaClientModel.ChangeChainAsync(selectedEndpoint);
        }

        public void UpdateNetworkInfos()
        {
            var tempOldValues = NetworkInfos;
            var networkInfos = new ObservableCollection<NetworkSelectInfo>();
            for (int i = 0; i < tempOldValues.Count; i++)
            {
                networkInfos.Add(new NetworkSelectInfo
                {
                    ShowName = tempOldValues[i].ShowName,
                    Name = tempOldValues[i].Name,
                    Icon = tempOldValues[i].Icon,
                    EndpointKey = tempOldValues[i].EndpointKey,
                    DarkIcon = tempOldValues[i].DarkIcon,
                    EndpointConnectionStatus = tempOldValues[i].EndpointConnectionStatus,
                    IsSelected = tempOldValues[i].IsSelected,
                });
            }

            NetworkInfos = networkInfos;
        }
    }
}

