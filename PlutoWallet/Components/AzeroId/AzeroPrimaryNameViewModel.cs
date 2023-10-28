using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model.AzeroId;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
using AzeroIdResolver;

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
			var temp = await TzeroId.GetPrimaryNameForAddress(client, KeysModel.GetSubstrateKey());

			if (temp == null) {

				PrimaryName = "None";
			}
			else
			{
				PrimaryName = temp.ToUpper();
				Tld = ("." + await TzeroId.GetTld(client)).ToUpper();
			}
		}
	}
}

