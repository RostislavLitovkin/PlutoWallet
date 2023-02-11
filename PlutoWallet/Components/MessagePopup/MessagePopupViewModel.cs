using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PlutoWallet.Components.MessagePopup
{
    public partial class MessagePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private bool isVisible;

        public MessagePopupViewModel()
        {
            isVisible = false;
        }
    }
}

