using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoWallet.Components.AddressView
{
    internal class CopyAddress
    {
        public static async Task CopyToClipboardAsync(string address)
        {
            await Clipboard.Default.SetTextAsync(address);
            var toast = Toast.Make("Copied to clipboard");
            await toast.Show();
        }
    }
}
