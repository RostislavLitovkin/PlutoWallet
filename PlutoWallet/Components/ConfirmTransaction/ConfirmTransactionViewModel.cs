using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.ConfirmTransaction
{
	public partial class ConfirmTransactionViewModel : ObservableObject
	{
		[ObservableProperty]
		private bool isVisible;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool errorIsVisible;

        [ObservableProperty]
        private ConfirmTransactionStatus status;

        public ConfirmTransactionViewModel()
		{
            Password = "";
            IsVisible = false;
            ErrorIsVisible = false;
            Status = ConfirmTransactionStatus.Waiting;
        }
	}

    public enum ConfirmTransactionStatus
    {
        Waiting,
        Verified,
        Denied,
    }
}

