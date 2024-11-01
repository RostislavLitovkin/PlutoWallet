namespace PlutoWallet.Components.Nft;

public partial class NftDetailPage : ContentPage
{
    public NftDetailPage(NftDetailViewModel viewModel)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        BindingContext = viewModel;
    }
}