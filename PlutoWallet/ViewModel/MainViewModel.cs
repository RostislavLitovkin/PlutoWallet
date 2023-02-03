using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plutonication;
using PlutoWallet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            TestConnect();
        }

        public MainViewModel()
        {
            TestListen();
        }

        private void TestConnect()
        {
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            Socket soc = ConnectionManager.Connect(ipAddr, port);
            if (soc != null)
            {
                Response = "socket connected";
            }
            else
            {
                Response = "socket failed to connect :(";
            }
        }
        private void TestListen()
        {
            int port = 8080;
            Socket soc = ConnectionManager.Listen(port);
            if (soc != null)
            {
                Response = "socket listening";
            }
            else
            {
                Response = "socket failed to listen :(";
            }
        }
    }
}
