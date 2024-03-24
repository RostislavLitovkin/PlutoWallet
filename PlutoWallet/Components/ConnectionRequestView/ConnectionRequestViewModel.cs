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
		private bool isVisible;

		[ObservableProperty]
		private string url;

        [ObservableProperty]
        private string key;

		[ObservableProperty]
		private bool requestViewIsVisible;

        [ObservableProperty]
        private bool connectionStatusIsVisible;

		[ObservableProperty]
		private bool checkIsVisible;

		[ObservableProperty]
		private string connectionStatusText;

        [ObservableProperty]
		private AccessCredentials accessCredentials;

        public ConnectionRequestViewModel()
		{
			requestViewIsVisible = true;
			connectionStatusIsVisible = false;
            isVisible = false;
			checkIsVisible = false;
			connectionStatusText = "Connecting";
        }

		public void Show()
		{
            RequestViewIsVisible = true;
            ConnectionStatusIsVisible = false;
            IsVisible = true;
			CheckIsVisible = false;
			ConnectionStatusText = "Connecting";
        }
    }
}

