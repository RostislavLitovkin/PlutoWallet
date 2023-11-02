using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;
using PlutoWallet.Model;

namespace PlutoWallet.Components.NetworkSelect
{
	public partial class NetworkSelectPopupViewModel : ObservableObject, IPopup
    {
		
		private bool isVisible;

		public bool IsVisible
		{
			get => isVisible;
			set
			{
				SetProperty(ref isVisible, value);

				if (!value)
				{
					// Save and change the endpoints
					List<string> keys = new List<string>();

					foreach (NetworkSelectInfo info in Networks)
					{
						if (info.IsSelected)
						{
							keys.Add(info.EndpointKey);
						}
					}

					Endpoints.SaveEndpoints(keys);
				}
			}
		}

        [ObservableProperty]
        private ObservableCollection<NetworkSelectInfo> networks = new ObservableCollection<NetworkSelectInfo>();

        public NetworkSelectPopupViewModel()
		{
			isVisible = false;
		}

		public void SetNetworks()
		{
            Dictionary<string, Endpoint> endpoints = Endpoints.GetEndpointDictionary;
			string[] keys = Endpoints.GetSelectedEndpointKeys();

			List<NetworkSelectInfo> tempNetworks = new List<NetworkSelectInfo>();

			foreach (string key in endpoints.Keys)
			{
				tempNetworks.Add(new NetworkSelectInfo
				{
					EndpointKey = key,
					Icon = endpoints[key].Icon,
					Name = endpoints[key].Name,
					IsSelected = keys.Contains(key),
				});
			}

			Networks = new ObservableCollection<NetworkSelectInfo>(tempNetworks);

			IsVisible = true;
        }

		public void SelectEndpoint(string key)
		{
			List<NetworkSelectInfo> tempNetworks = new List<NetworkSelectInfo>(Networks);
			List<string> selectedKeys = new List<string>();

			for(int i = 0; i < tempNetworks.Count(); i++)
			{
				if (tempNetworks[i].EndpointKey == key)
				{
                    tempNetworks[i].IsSelected = !tempNetworks[i].IsSelected;
				}

				if (tempNetworks[i].IsSelected)
				{
					selectedKeys.Add(tempNetworks[i].EndpointKey);
				}
			}

			Networks = new ObservableCollection<NetworkSelectInfo>(tempNetworks);
		}
	}
}

