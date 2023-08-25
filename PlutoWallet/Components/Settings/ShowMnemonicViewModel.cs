using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.Settings
{
	public partial class ShowMnemonicViewModel : ObservableObject
	{
		[ObservableProperty]
		private string title;

		public ShowMnemonicViewModel()
		{
			title = Preferences.Get("usePrivateKey", false) ? "Show SecretKey (click 10 times)" : "Show Mnemonics (click 10 times)";
		}
	}
}

