namespace PlutoWallet.Components.AddressView;

public partial class ChainAddressView : ContentView
{
	public ChainAddressView()
	{
        BindingContext = DependencyService.Get<ChainAddressViewModel>();

        InitializeComponent();
    }
}
