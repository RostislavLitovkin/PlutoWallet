using ZXing.Net.Maui.Controls;


namespace PlutoWallet.Components.PublicKeyQrCode;

public partial class PublicKeyQrCode : ContentView
{
	public PublicKeyQrCode()
	{
		InitializeComponent();

        PublicKeyQrCode publicKeyQrCode = DependencyService.Get<PublicKeyQrCode>();
        BindingContext = publicKeyQrCode;

    }
}
