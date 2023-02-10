using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.TransferView
{
	internal partial class TransferViewModel : ObservableObject
	{
		[ObservableProperty]
		private string address;

		[ObservableProperty]
		private int amount;

    }
}

