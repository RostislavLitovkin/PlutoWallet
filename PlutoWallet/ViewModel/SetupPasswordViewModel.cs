using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.ViewModel
{
	public partial class SetupPasswordViewModel : ObservableObject
	{
        [ObservableProperty]
        private string password;

        public SetupPasswordViewModel()
        {
            password = "";
        }
    }
}

