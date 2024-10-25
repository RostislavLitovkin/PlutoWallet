using PlutoWallet.Components.TransactionAnalyzer;
using PlutoWallet.Model;
using Substrate.NetApi.Model.Extrinsics;
using UniqueryPlus.Nfts;

namespace PlutoWallet.Components.Nft;

public partial class NftTransferView : ContentView
{
    public NftTransferView()
	{
		InitializeComponent();

        var viewModel = DependencyService.Get<NftTransferViewModel>();

        BindingContext = viewModel;
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        var viewModel = DependencyService.Get<NftTransferViewModel>();

        viewModel.SetToDefault();
    }
}