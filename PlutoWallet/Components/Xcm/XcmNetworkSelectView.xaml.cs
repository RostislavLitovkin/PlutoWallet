namespace PlutoWallet.Components.Xcm;

public partial class XcmNetworkSelectView : ContentView
{
	public XcmNetworkSelectView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<XcmNetworkSelectViewModel>();
    }

    void OnOriginClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<XcmNetworkSelectPopupViewModel>();

        viewModel.XcmLocation = XcmLocation.Origin;

        viewModel.SetNetworks();
    }

    void OnDestinationClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<XcmNetworkSelectPopupViewModel>();

        viewModel.XcmLocation = XcmLocation.Destination;

        viewModel.SetNetworks();
    }
}
