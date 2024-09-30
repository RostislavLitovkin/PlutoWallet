using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Xcm
{
	public partial class XcmTransferViewModel : ObservableObject
    {
		[ObservableProperty]
		private decimal amount; 

		public XcmTransferViewModel()
		{
			amount = 0;
		}
	}
}

