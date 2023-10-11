using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.Model.SubSquare;

namespace PlutoWallet.Components.Referenda
{
	public partial class ReferendaViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<Root> referenda = new ObservableCollection<Root>();

		public ReferendaViewModel()
		{

		}

		public async Task GetReferenda()
		{
			List<Root> tempReferenda = new List<Root>();

			tempReferenda.Add(await Model.SubSquare.ReferendumModel.GetReferendumInfo(177));

			Referenda = new ObservableCollection<Root>(tempReferenda);
		}
	}
}

