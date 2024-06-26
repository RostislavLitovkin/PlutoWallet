using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Constants;
using PlutoWallet.Model;

namespace PlutoWallet.Components.Xcm
{
	public partial class XcmNetworkSelectPopupViewModel : ObservableObject, IPopup
    {
        private bool isVisible;

        public bool IsVisible
        {
            get => isVisible;
            set
            {
                SetProperty(ref isVisible, value);
            }
        }

        [ObservableProperty]
        private XcmLocation xcmLocation;

        [ObservableProperty]
        private string originKey;

        [ObservableProperty]
        private string destinationKey;

        [ObservableProperty]
        private ObservableCollection<XcmNetworkSelectInfo> networks = new ObservableCollection<XcmNetworkSelectInfo>();

        public XcmNetworkSelectPopupViewModel()
        {
            xcmLocation = XcmLocation.Origin;
            isVisible = false;

            originKey = "polkadot";
            destinationKey = "statemint";
        }

        public void SetNetworks()
        {
            Console.WriteLine("SetNetwork()");
            Console.WriteLine(XcmLocation);

            var endpoints = Endpoints.GetEndpointDictionary;

            List<XcmNetworkSelectInfo> tempNetworks = new List<XcmNetworkSelectInfo>();

            string selectedKey = this.XcmLocation == XcmLocation.Origin ? OriginKey : DestinationKey;

            Console.WriteLine(selectedKey);

            foreach (string key in endpoints.Keys)
            {
                if (endpoints[key].ParachainId == null)
                {
                    continue;
                }

                tempNetworks.Add(new XcmNetworkSelectInfo
                {
                    EndpointKey = key,
                    Icon = endpoints[key].Icon,
                    DarkIcon = endpoints[key].DarkIcon,
                    Name = endpoints[key].Name,
                    IsSelected = selectedKey == key,
                    EndpointConnectionStatus = EndpointConnectionStatus.Loading,
                    ParachainId = endpoints[key].ParachainId,
                });

                if (selectedKey == key)
                {
                    Console.WriteLine(endpoints[key].Name);
                }
            }

            Networks = new ObservableCollection<XcmNetworkSelectInfo>(tempNetworks);

            IsVisible = true;
        }

        public void SelectEndpoint(string key)
        {
            Console.WriteLine("SelectEndpoint");

            var viewModel = DependencyService.Get<XcmNetworkSelectViewModel>();

            var endpoint = Endpoints.GetEndpointDictionary[key];

            if (this.XcmLocation == XcmLocation.Origin)
            {
                OriginKey = key;

                viewModel.OriginName = endpoint.Name;

                viewModel.OriginIcon = endpoint.Icon;
            }
            else if (this.XcmLocation == XcmLocation.Destination)
            {
                DestinationKey = key;

                viewModel.DestinationName = endpoint.Name;

                viewModel.DestinationIcon = endpoint.Icon;
            }

            IsVisible = false;
        }
    }
}

