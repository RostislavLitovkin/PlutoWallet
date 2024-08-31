using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.AddressView
{
    internal partial class AddressQrCodeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string qrAddress;

        private string address;

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value.Substring(0, value.Length / 2) + "\n" + value.Substring(value.Length / 2));
        }

        [ObservableProperty]
        private bool isVisible;

        public AddressQrCodeViewModel()
        {
            isVisible = false;
        }
    }
}
