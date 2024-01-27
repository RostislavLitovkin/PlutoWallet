using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Fee
{
	public partial class FeeAssetViewModel : ObservableObject
	{
		[ObservableProperty]
		private string asset;

		public FeeAssetViewModel()
		{
			asset = "Loading";
		}
	}
}

