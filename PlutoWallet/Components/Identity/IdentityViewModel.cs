using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Identity
{
	public partial class IdentityViewModel : ObservableObject
	{
		[ObservableProperty]
		private string name;

		public IdentityViewModel()
		{
		}

		public async Task GetIdentity()
		{
			Name = "Loading";
			try
			{
				var identity = await Model.IdentityModel.GetIdentityForAddress(Model.KeysModel.GetSubstrateKey());

				Name = identity.DisplayName;
			}
			catch(Exception ex)
			{
				Name = ex.Message;
			}
        }
	}
}

