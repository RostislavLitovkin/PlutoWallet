using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.ChangeLayoutRequest
{
	public partial class ChangeLayoutRequestViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

		public ChangeLayoutRequestViewModel()
		{
			isVisible = false;
		}

		
	}
}

