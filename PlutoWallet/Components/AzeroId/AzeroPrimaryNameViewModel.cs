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

		[ObservableProperty]
		private string reservedUntil;

		[ObservableProperty]
		private bool reservedUntilIsVisible;

		public AzeroPrimaryNameViewModel()
		{
			primaryName = "Loading";
			reservedUntilIsVisible = false;
		}

		public async Task GetPrimaryName(PlutoWalletSubstrateClient client)
		{
			if (client is null)
			{
				return;
			}

			var temp = await TzeroId.GetPrimaryNameForAddress(client.SubstrateClient, KeysModel.GetSubstrateKey());

			if (temp == null)
			{

				PrimaryName = "None";
				ReservedUntilIsVisible = false;
			}
			else
			{
				PrimaryName = temp.ToUpper();
				Tld = ("." + await TzeroId.GetTld(client.SubstrateClient)).ToUpper();

				ReservedUntil = await Model.AzeroId.AzeroIdModel.GetReservedUntilStringForName(temp);

				ReservedUntilIsVisible = true;
			}
		}
	}
}

