namespace PlutoWallet.Components.Nft;

public partial class NftLoadingView : ContentView
{
	public NftLoadingView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<NftLoadingViewModel>();
    }
}
