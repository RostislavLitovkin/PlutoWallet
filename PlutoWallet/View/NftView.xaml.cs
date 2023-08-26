using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class NftView : ContentView
{
	public NftView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<NftViewModel>();

        DependencyService.Get<NftViewModel>().GetNFTsAsync();
    }
}
