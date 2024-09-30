using System;
using PlutoWallet.Model;
using PlutoWallet.Constants;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.CalamarView
{
	public partial class CalamarViewModel : ObservableObject
	{
		[ObservableProperty]
		private string webAddress;

		public CalamarViewModel()
		{

		}

		public void Reload(Endpoint endpoint)
		{
			string address = KeysModel.GetPublicKey();

			if (endpoint.CalamarChainName == null)
			{
                // Not supported
            }

            WebAddress = "https://calamar.app/" + endpoint.CalamarChainName + "/account/" + address;
        }
	}
}

