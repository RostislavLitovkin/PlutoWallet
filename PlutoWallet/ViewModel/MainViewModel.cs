using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlutoWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWallet.ViewModel
{
    internal partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string response;

        [RelayCommand]
        private void IncrementCounter()
        {
            //Response = NetworkingModel.RequestSample();
            //Response = KeysModel.GenerateMnemonicsArray();
        }

        public MainViewModel()
        {
            response = "request me ^^";
        }
    }
}
