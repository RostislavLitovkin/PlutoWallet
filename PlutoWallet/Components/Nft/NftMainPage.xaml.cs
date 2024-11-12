namespace PlutoWallet.Components.Nft;

public partial class NftMainPage : ContentPage
{
	public NftMainPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        var viewModel = new NftMainViewModel();

        BindingContext = viewModel;

        viewModel.ConnectClientsAsync(CancellationToken.None);
    }
}