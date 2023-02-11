using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.ScannerView
{
	public partial class ScannerViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isScanning;

		public ScannerViewModel()
		{
			isScanning = true;
		}
	}
}

