using ZXing.Net.Maui;

namespace PlutoWallet.Components.ScannerView;

public partial class ScannerView : ContentView
{
	/**
	 * This one is used, when you want to use Dependency service
	 */
	public ScannerView(ScannerViewModel bindingContext)
	{
		InitializeComponent();

		BindingContext = bindingContext;

		scanner.Options = new BarcodeReaderOptions
		{
			Formats = BarcodeFormats.TwoDimensional,
		};
    }

    public ScannerView()
    {
        InitializeComponent();

		BindingContext = new ScannerViewModel();

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
}
