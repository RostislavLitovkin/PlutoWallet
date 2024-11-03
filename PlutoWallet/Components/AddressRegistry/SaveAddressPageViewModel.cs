using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Substrate.NetApi;
using static Substrate.NetApi.Utils;

namespace PlutoWallet.Components.AddressRegistry
{
    public class SaveAddressPageViewModel : INotifyPropertyChanged
    {
        private string _address;
        private string _nickname;
        private bool _isAddressValid;

        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    ValidateAddress();
                    OnPropertyChanged();
                    ((Command)SaveCommand).ChangeCanExecute();
                }
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                if (_nickname != value)
                {
                    _nickname = value;
                    OnPropertyChanged();
                    ((Command)SaveCommand).ChangeCanExecute();
                }
            }
        }

        public bool IsAddressValid
        {
            get => _isAddressValid;
            private set
            {
                if (_isAddressValid != value)
                {
                    _isAddressValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand RejectCommand { get; }

        public SaveAddressPageViewModel(string initialAddress)
        {
            Address = initialAddress;
            SaveCommand = new Command(
                execute: async () => 
                {
                    Preferences.Set(Address, Nickname);
                    await Shell.Current.Navigation.PopAsync();
                },
                canExecute: () => IsAddressValid && !string.IsNullOrWhiteSpace(Nickname)
            );
            
            RejectCommand = new Command(async () => await Shell.Current.Navigation.PopAsync());
        }

        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
            {
                IsAddressValid = false;
                return;
            }

            try
            {
                Utils.GetPublicKeyFrom(Address, out short _);
                IsAddressValid = true;
            }
            catch
            {
                IsAddressValid = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}