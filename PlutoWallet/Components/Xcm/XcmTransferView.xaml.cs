namespace PlutoWallet.Components.Xcm;

public partial class XcmTransferView : ContentView
{
	public XcmTransferView()
	{
		InitializeComponent();
	}

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		await Navigation.PushAsync(new XcmTransferPage());
    }
}
