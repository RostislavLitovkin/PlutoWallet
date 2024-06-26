using System;
using CommunityToolkit.Mvvm.ComponentModel;
using Plutonication;

namespace PlutoWallet.Components.ConnectionRequestView
{
	public partial class ConnectionRequestViewModel : ObservableObject
	{
		[ObservableProperty]
		private string name;

		[ObservableProperty]
		private string icon;

		[ObservableProperty]
		private string url;

        [ObservableProperty]
        private string key;

        [ObservableProperty]
        private string plutoLayout;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
		private bool requestViewIsVisible;

        [ObservableProperty]
        private bool connectionStatusIsVisible;

        [ObservableProperty]
        private bool connecting;

        [ObservableProperty]
		private bool connected;

        [ObservableProperty]
        private bool confirming;

        [ObservableProperty]
        private bool confirmed;

        [ObservableProperty]
		private string connectionStatusText;

        [ObservableProperty]
		private AccessCredentials accessCredentials;

        public ConnectionRequestViewModel()
		{
			requestViewIsVisible = true;
			connectionStatusIsVisible = false;
            isVisible = false;
            connecting = false;
            connected = false;
            confirming = false;
            confirmed = false;
            connectionStatusText = "Connecting";
        }

		public void Show()
		{
            RequestViewIsVisible = true;
            ConnectionStatusIsVisible = false;
            IsVisible = true;
            Connecting = false;
            Connected = false;
            Confirming = false;
            Confirmed = false;
            ConnectionStatusText = "Connecting";
        }
    }
}

