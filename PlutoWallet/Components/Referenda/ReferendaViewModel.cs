using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using PlutoWallet.Model.SubSquare;

namespace PlutoWallet.Components.Referenda
{
	public partial class ReferendaViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<ReferendumInfo> referenda = new ObservableCollection<ReferendumInfo>();

		[ObservableProperty]
		private string loading;

		public ReferendaViewModel()
		{
			loading = "Loading";
		}

		public async Task GetReferenda()
		{
			Loading = "Loading";

			var referenda = await Model.SubSquare.ReferendumModel.GetReferenda(AjunaClientModel.GroupClients, KeysModel.GetSubstrateKey());
			if (referenda.Count() == 0)
			{
				Loading = "None";
				return;
			}

			Loading = "";

            Referenda = new ObservableCollection<ReferendumInfo>(referenda);


		}
	}
}

