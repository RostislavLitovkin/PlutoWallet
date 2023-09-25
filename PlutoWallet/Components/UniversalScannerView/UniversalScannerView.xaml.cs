using System.Web;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.ScannerView;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;
using PlutoWallet.ViewModel;
using Plutonication;

namespace PlutoWallet.Components.UniversalScannerView;

public partial class UniversalScannerView : ContentView
{
    ScannerView.ScannerView scanner;

    public UniversalScannerView()
    {
        InitializeComponent();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        qrLayout.Children.Clear();

        // Hide this layout
        await this.FadeTo(0, 500);
        this.IsVisible = false;
    }

    public async Task Appear()
    {
        this.IsVisible = true;

        await this.FadeTo(1, 500);

        scanner = new ScannerView.ScannerView()
        {
            OnScannedMethod = OnScanned
        };

        qrLayout.Children.Add(scanner);
    }


    async void OnScanned(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        if (e.Results.Length <= 0)
        {
            return;
        }

        try
        {
            var scannedValue = e.Results[0].Value;

            // trying to connect to a dApp
            if (scannedValue.Length > 14 && scannedValue.Substring(0, 14) == "plutonication:")
            {
                AccessCredentials ac = new AccessCredentials(new Uri(scannedValue));

                var connectionRequest = DependencyService.Get<ConnectionRequestViewModel>();

                connectionRequest.IsVisible = true;
                connectionRequest.Icon = ac.Icon;
                connectionRequest.Name = ac.Name;

                connectionRequest.Url = ac.Url;
                connectionRequest.Key = ac.Key;
                connectionRequest.AccessCredentials = ac;

            }
            else if (scannedValue.Length > 13 && scannedValue.Substring(0, 13) == "plutolayout: ")
            {
                // LATER: check validity

                Model.CustomLayoutModel.SaveLayout(scannedValue);

                Model.CustomLayoutModel.ShowImportSuccessfulRestartNeededMessage();
            }
            else if (scannedValue.Length > 3 && scannedValue.Substring(0, 3) == "wc:")
            {
                Console.WriteLine(scannedValue);
                await Model.WalletConnectModel.Connect(scannedValue);
            }
            else
            {
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Unable to read QR code";
                messagePopup.Text = "The QR code was in incorrect format.";

                messagePopup.IsVisible = true;
            }

            qrLayout.Children.Clear();

            this.IsVisible = false;
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Error";
            messagePopup.Text = ex.Message;

            messagePopup.IsVisible = true;
        }
    }
}
