using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model.AzeroId;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;

namespace PlutoWallet.Components.AzeroId
{
	public partial class AzeroPrimaryNameViewModel : ObservableObject
	{
		[ObservableProperty]
		private string primaryName;

		[ObservableProperty]
		private string tld;

		public AzeroPrimaryNameViewModel()
		{
			primaryName = "Loading";
		}

		public async Task GetPrimaryName(SubstrateClientExt client)
		{
			var temp = await AzeroIdModel.GetPrimaryNameForAddress(client, KeysModel.GetSubstrateKey());

			if (temp == null) {

				PrimaryName = "None";
			}
			else
			{
				PrimaryName = temp.ToUpper();
				Tld = ("." + await AzeroIdModel.GetTld(client)).ToUpper();
			}
		}
	}
}

