using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Constants;

namespace PlutoWallet.Components.Xcm
{
	public partial class XcmNetworkSelectViewModel : ObservableObject
	{
		[ObservableProperty]
		private string originName;

        [ObservableProperty]
        private string originIcon;

        [ObservableProperty]
        private string destinationName;

        [ObservableProperty]
        private string destinationIcon;

        public XcmNetworkSelectViewModel()
		{

            originName = Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot].Name;
            originIcon = Endpoints.GetEndpointDictionary[EndpointEnum.Polkadot].Icon;

            destinationName = Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub].Name;
            destinationIcon = Endpoints.GetEndpointDictionary[EndpointEnum.PolkadotAssetHub].Icon;
        }
	}
}

