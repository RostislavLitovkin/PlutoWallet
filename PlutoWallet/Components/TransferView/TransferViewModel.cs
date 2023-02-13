using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.TransferView
{
	public partial class TransferViewModel : ObservableObject
	{
		[ObservableProperty]
		private string address;

		[ObservableProperty]
		private int amount;

		[ObservableProperty]
		private bool isVisible;

		public TransferViewModel()
		{
			isVisible = false;
		}
    }
}

