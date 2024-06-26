namespace PlutoWallet.Components.Xcm;

public partial class XcmNetworkSelectPopup : ContentView
{
	public XcmNetworkSelectPopup()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<XcmNetworkSelectPopupViewModel>();
    }
}
