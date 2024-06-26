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

            originName = Endpoints.GetEndpointDictionary["polkadot"].Name;
            originIcon = Endpoints.GetEndpointDictionary["polkadot"].Icon;

            destinationName = Endpoints.GetEndpointDictionary["statemint"].Name;
            destinationIcon = Endpoints.GetEndpointDictionary["statemint"].Icon;
        }
	}
}

