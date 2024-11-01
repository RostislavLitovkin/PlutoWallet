using PlutoWallet.Components.WebView;
using UniqueryPlus.External;

namespace PlutoWallet.Components.Nft;

public partial class CollectionDetailPage : ContentPage
{
    private CollectionDetailViewModel viewModel;
    public CollectionDetailPage(CollectionDetailViewModel viewModel)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        this.viewModel = viewModel;

        BindingContext = viewModel;
    }

    private async void OnUniqueClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebViewPage(((IUniqueMarketplaceLink)viewModel.CollectionBase).UniqueMarketplaceLink));
    }

    private async void OnKodaClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebViewPage(((IKodaLink)viewModel.CollectionBase).KodaLink));
    }
}