using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.DAppConnectionView
{
    public partial class DAppConnectionViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string icon;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private string connectionState;

        [ObservableProperty]
        private Color connectionStateColor;

        public DAppConnectionViewModel()
        {
            /*isVisible = true;
            name = "Galaxy Logic Game";
            icon = "https://rostislavlitovkin.pythonanywhere.com/logo";*/

            isVisible = false;
        }

        public void SetConnectionState(DAppConnectionStateEnum state)
        {
            switch (state)
            {
                case DAppConnectionStateEnum.Waiting:
                    ConnectionState = "Waiting";
                    ConnectionStateColor = Colors.Gray;
                    break;
                case DAppConnectionStateEnum.Connecting:
                    ConnectionState = "Connecting";
                    ConnectionStateColor = Colors.Orange;
                    break;
                case DAppConnectionStateEnum.Confirming:
                    ConnectionState = "Confirming";
                    ConnectionStateColor = Colors.Green;
                    break;
                case DAppConnectionStateEnum.Connected:
                    ConnectionState = "Connected";
                    ConnectionStateColor = Colors.Green;
                    break;
                case DAppConnectionStateEnum.Rejected:
                    ConnectionState = "Rejected";
                    ConnectionStateColor = Colors.DarkRed;
                    break;
                case DAppConnectionStateEnum.Disconnected:
                    ConnectionState = "Disconnected";
                    ConnectionStateColor = Colors.DarkRed;
                    break;
                case DAppConnectionStateEnum.Reconnecting:
                    ConnectionState = "Reconnecting";
                    ConnectionStateColor = Colors.Orange;
                    break;
            }
        }
    }
}

