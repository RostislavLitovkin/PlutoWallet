using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Components.WebView;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using UniqueryPlus.External;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft;

public partial class NftDetailPage : ContentPage
{
    private NftDetailViewModel viewModel;
    public NftDetailPage(NftDetailViewModel viewModel)
    {
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        this.viewModel = viewModel;

        BindingContext = viewModel;
    }

    private async void OnTransferClicked(object sender, EventArgs e)
    {
        var nftTransferViewModel = DependencyService.Get<NftTransferViewModel>();

        nftTransferViewModel.EndpointKey = viewModel.Endpoint.Key;
        nftTransferViewModel.NftBase = this.viewModel.NftBase;
        nftTransferViewModel.IsVisible = true;
        await nftTransferViewModel.GetFeeAsync(viewModel.Endpoint.Key, viewModel.NftBase);
    }

    private void OnModifyClicked(object sender, EventArgs e)
    {

    }

    private async void OnBurnClicked(object sender, EventArgs e)
    {
        var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(viewModel.Endpoint.Key);

        var client = clientExt.SubstrateClient;

        try
        {
            Method transfer = ((INftBurnable)viewModel.NftBase).Burn();

            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            await transactionAnalyzerConfirmationViewModel.LoadAsync(clientExt, transfer, false, onConfirm: OnBurnConfirmClicked);
        }
        catch (Exception ex)
        {
            //errorLabel.Text = ex.Message;
            Console.WriteLine(ex);
        }
    }

    public static async Task OnBurnConfirmClicked()
    {
        if ((await KeysModel.GetAccount()).IsSome(out var account))
        {
            var transactionAnalyzerConfirmationViewModel = DependencyService.Get<TransactionAnalyzerConfirmationViewModel>();

            var clientExt = await Model.AjunaClientModel.GetOrAddSubstrateClientAsync(transactionAnalyzerConfirmationViewModel.Endpoint.Key);

            try
            {
                string extrinsicId = await clientExt.SubmitExtrinsicAsync(transactionAnalyzerConfirmationViewModel.Payload.Call, account, token: CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed at confirm clicked");
                Console.WriteLine(ex);
            }

            /// Hide

            transactionAnalyzerConfirmationViewModel.IsVisible = false;
        }
        else
        {
            // Verification failed, do something about it
        }
    }

    private async void OnUniqueClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebViewPage(((IUniqueMarketplaceLink)viewModel.NftBase).UniqueMarketplaceLink));
    }

    private async void OnKodaClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebViewPage(((IKodaLink)viewModel.NftBase).KodaLink));
    }
}