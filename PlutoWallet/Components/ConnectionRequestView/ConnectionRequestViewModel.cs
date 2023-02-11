using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.ConnectionRequestView
{
	public partial class ConnectionRequestViewModel : ObservableObject
	{
		[ObservableProperty]
		private string name;

		[ObservableProperty]
		private string icon;

		[ObservableProperty]
		private bool isVisible;

		public ConnectionRequestViewModel()
		{
			isVisible = false;
		}
	}
}

