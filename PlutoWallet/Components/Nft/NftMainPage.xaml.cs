namespace PlutoWallet.Components.Nft;

public partial class NftMainPage : ContentPage
{
	public NftMainPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        var viewModel = DependencyService.Get<NftMainViewModel>();

        BindingContext = viewModel;
    }
}