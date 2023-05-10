using System;
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

        public DAppConnectionViewModel()
        {
            /*isVisible = true;
            name = "Galaxy Logic Game";
            icon = "https://rostislavlitovkin.pythonanywhere.com/logo";*/

            isVisible = false;
        }
    }
}

