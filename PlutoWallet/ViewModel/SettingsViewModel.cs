using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using CommunityToolkit.Mvvm.ComponentModel;
using Plutonication;

namespace PlutoWallet.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string test1 = string.Empty;
        [ObservableProperty]
        private string test2 = string.Empty;

        public SettingsViewModel()
        {
            TestIpFinding();
        }

        private void TestIpFinding()
        {

            var result = new List<IPAddress>();
            try
            {
                var upAndNotLoopbackNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                                                                                              && n.OperationalStatus == OperationalStatus.Up);

                foreach (var networkInterface in upAndNotLoopbackNetworkInterfaces)
                {
                    var iPInterfaceProperties = networkInterface.GetIPProperties();

                    var unicastIpAddressInformation = iPInterfaceProperties.UnicastAddresses.FirstOrDefault(u => u.Address.AddressFamily == AddressFamily.InterNetwork);
                    if (unicastIpAddressInformation == null) continue;

                    result.Add(unicastIpAddressInformation.Address);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to find IP: {ex.Message}");
            }
            finally
            {
                string allIpInfo = string.Empty;
                foreach (var item in result)
                {
                    allIpInfo += item.ToString() + "---";
                }
                Test1 = result.FirstOrDefault()?.ToString();

                //Test2 = PlutoManager.GetMyIpAddress().ToString();
            }
        }
    }
}

