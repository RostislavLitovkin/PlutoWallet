
using ZXing.Net.Maui;

namespace PlutoWallet.Components.ScannerView;

public partial class ScannerView : ContentView
{
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

    public bool HideWhenDetected
	{
		set
		{
			if(value)
			{
                //cameraBarcodeReaderView.BarcodesDetected += HideWhenDetectedEvent;
            }
		}
	}

	public EventHandler<BarcodeDetectionEventArgs> OnScannedMethod
	{
		set
		{
			scanner.BarcodesDetected += value;
        }
	}

    async void HideWhenDetectedEvent(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
		//
    }

}
