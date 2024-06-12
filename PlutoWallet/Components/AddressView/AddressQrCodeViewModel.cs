using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.AddressView
{
    internal partial class AddressQrCodeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string qrAddress;

        [ObservableProperty]
        private string address;

        [ObservableProperty]
        private bool isVisible;

        public AddressQrCodeViewModel()
        {
            isVisible = false;
        }
    }
}
