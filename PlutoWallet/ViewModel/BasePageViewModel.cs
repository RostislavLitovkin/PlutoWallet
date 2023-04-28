using System;
using CommunityToolkit.Mvvm.ComponentModel;
using PlutoWallet.View;

namespace PlutoWallet.ViewModel
{
	public partial class BasePageViewModel : ObservableObject
	{
		[ObservableProperty]
		private ContentView content;

		public BasePageViewModel()
		{
			content = new MainView();
		}
	}
}

