using System;
using PlutoWallet.View;

namespace PlutoWallet.ViewModel
{
	public class ShellViewModel
	{
		public object ShellContent
		{
			get
			{
				if (Preferences.ContainsKey("privateKey"))
				{
					return new MainPage();

				}
				return new MnemonicsPage();
			}
		}

		public ShellViewModel()
		{
		}
	}
}

