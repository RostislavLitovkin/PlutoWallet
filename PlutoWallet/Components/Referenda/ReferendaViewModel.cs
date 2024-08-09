using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
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

		public async Task GetReferenda(CancellationToken token)
		{
			Loading = "Loading";

			foreach (var client in AjunaClientModel.Clients.Values)
            {
                await Model.SubSquare.ReferendumModel.GetReferendaAsync(await client.Task, KeysModel.GetSubstrateKey(), token);
            }

            if (Model.SubSquare.ReferendumModel.Referenda.Count() == 0)
			{
				Loading = "None";
				return;
			}

			Loading = "";

            Referenda = new ObservableCollection<ReferendumInfo>(ReferendumModel.Referenda.Values);


		}
	}
}

