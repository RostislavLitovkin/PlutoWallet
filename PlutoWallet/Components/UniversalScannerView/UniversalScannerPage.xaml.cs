using ZXing.Net.Maui;

namespace PlutoWallet.Components.UniversalScannerView;

public partial class UniversalScannerPage : ContentPage
{
	public UniversalScannerPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        scanner.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
        };
    }

    public EventHandler<BarcodeDetectionEventArgs> OnScannedMethod
    {
        set
        {
            scanner.BarcodesDetected += value;
        }
    }

    private void OnDetected(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        scanner.IsDetecting = false;
    }
}
