using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.CustomLayouts
{
	public partial class CustomItemViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

		[ObservableProperty]
		private ContentView content;

        [ObservableProperty]
        private string itemName;

        public CustomItemViewModel()
		{
			isVisible = false;
		}
	}
}

