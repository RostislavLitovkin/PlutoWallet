using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.CustomLayouts
{
	public partial class ExportPlutoLayoutQRViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

		[ObservableProperty]
		private string plutoLayoutValue;

		public ExportPlutoLayoutQRViewModel()
		{
		}
	}
}

