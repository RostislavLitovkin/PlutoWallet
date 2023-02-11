using System.Web;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.ScannerView;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace PlutoWallet.Components.UniversalScannerView;

public partial class UniversalScannerView : ContentView
{
    ScannerView.ScannerView scanner;

    public UniversalScannerView()
    {
        InitializeComponent();
#if ANDROID || IOS

        

#endif
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

        var viewModel = DependencyService.Get<ViewModel.MainViewModel>();

        try
        {
            // this is just for debugging
            viewModel.DAppName = e.Results[0].Value;


            var scannedValue = e.Results[0].Value;

            // trying to connect to a dApp
            if (scannedValue.Length > 14 && scannedValue.Substring(0, 14) == "plutonication:")
            {
                Uri myUri = new Uri(scannedValue);

                string url = HttpUtility.ParseQueryString(myUri.Query).Get("url");
                string icon = HttpUtility.ParseQueryString(myUri.Query).Get("icon");
                string name = HttpUtility.ParseQueryString(myUri.Query).Get("name");
                string key = HttpUtility.ParseQueryString(myUri.Query).Get("key");

                var connectionRequest = DependencyService.Get<ConnectionRequestViewModel>();

                connectionRequest.Icon = icon;
                connectionRequest.Name = name;

                connectionRequest.IsVisible = true;
            }
            else
            {
                viewModel.DAppName = "We know we failed";
                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "Unable to read QR code";
                messagePopup.Text = "Thq QR code was in incorrect format.";

                messagePopup.IsVisible = true;
            }

            qrLayout.Children.Clear();

            await this.FadeTo(0, 500);
            this.IsVisible = false;
        }
        catch (Exception ex)
        {
            viewModel.MetadataLabel = ex.Message;
        }
    }
}
