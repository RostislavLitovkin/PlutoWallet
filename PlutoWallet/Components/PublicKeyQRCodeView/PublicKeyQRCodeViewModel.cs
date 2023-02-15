using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.PublicKeyQRCodeView
{
    public partial class PublicKeyQRCodeViewModel : ObservableObject
    {
        [ObservableProperty]
        private string publicKey;

        [ObservableProperty]
        private bool isVisible;

        public PublicKeyQRCodeViewModel()
        {
            isVisible = false;
        }
    }
}

