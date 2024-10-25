using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.WebView;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using UniqueryPlus.Collections;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;

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