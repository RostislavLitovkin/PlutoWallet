using PlutoWallet.Model;
using System.Collections.ObjectModel;
using UniqueryPlus;

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

        Task s = viewModel.ConnectClientsAsync(CancellationToken.None);
    }

    private async void OnClaimDiamondsClicked(object sender, EventArgs e)
    {
        CancellationToken token = CancellationToken.None;

        try
        {
            var client = await AjunaClientModel.GetOrAddSubstrateClientAsync(Constants.EndpointEnum.Opal);

            var viewModel = new CollectionDetailViewModel();

            var collectionBase = await UniqueryPlus.Collections.CollectionModel.GetCollectionByCollectionIdAsync(client.SubstrateClient, NftTypeEnum.Opal, 4557, CancellationToken.None);

            viewModel.Endpoint = client.Endpoint;
            viewModel.CollectionId = collectionBase.CollectionId;
            viewModel.Favourite = false;
            viewModel.OwnerAddress = collectionBase.Owner;

            await CollectionThumbnailView.UpdateViewModelAsync(viewModel, collectionBase, token);

            await Navigation.PushAsync(new CollectionDetailPage(viewModel));

            var fullCollection = await collectionBase.GetFullAsync(token);

            await CollectionThumbnailView.UpdateViewModelAsync(viewModel, fullCollection, token);

            viewModel.Nfts = new ObservableCollection<NftWrapper>((await fullCollection.GetNftsAsync(25, null, token)).Select(Model.NftModel.ToNftWrapper));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}