using System;
using Ajuna.NetApi;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using static Ajuna.NetApi.Mnemonic;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;

namespace PlutoWallet.ViewModel
{
    public partial class MnemonicsViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [ObservableProperty]
        private string[] mnemonicsArray;

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (password == value)
                    return;

                password = value;

                // tell the user that his password is weak

                RaisePropertyChanged(nameof(Password));
                RaisePropertyChanged(nameof(IsStrongPassword));
            }
        }

        public bool IsStrongPassword => !(Password == null || Password == "");

        public void Continue()
        {
            var mnemonicsString = string.Empty;
            foreach (var item in MnemonicsArray)
            {
                mnemonicsString += item + " ";
            }

            Preferences.Set(
                "privateKey",
                Mnemonic.GetSecretKeyFromMnemonic(mnemonicsString, Password, BIP39Wordlist.English).ToString()
            );
        }

        public MnemonicsViewModel()
        {
            mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

