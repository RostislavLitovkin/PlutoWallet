using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.TransactionRequest
{
	public partial class MessageSignRequestViewModel : ObservableObject
    {
        [ObservableProperty]
        private string messageString;

        [ObservableProperty]
        private Plutonication.RawMessage message;

        [ObservableProperty]
        private bool isVisible;

        public MessageSignRequestViewModel()
		{
            isVisible = false;
		}
	}
}

