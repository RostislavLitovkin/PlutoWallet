using PlutoWallet.Model;
using PlutoWallet.Constants;
using PlutoWallet.Components.Nft;
using PlutoWallet.ViewModel;
using System.Windows.Input;

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
