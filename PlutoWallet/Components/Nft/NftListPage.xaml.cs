using PlutoWallet.ViewModel;

namespace PlutoWallet.Components.Nft;

public partial class NftListPage : ContentPage
{
	public NftListPage(object bindingContext)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        BindingContext = bindingContext;
    }
}