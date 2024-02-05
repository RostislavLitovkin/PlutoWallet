using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Model;
using Substrate.NetApi.Generated.Model.pallet_assets.types;

namespace PlutoWallet.Components.Balance;

public partial class UsdBalanceView : ContentView
{
	public UsdBalanceView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<UsdBalanceViewModel>();
    }

    async void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

        usdBalanceViewModel.UsdSum = "Loading";

        usdBalanceViewModel.ReloadIsVisible = false;

        await Model.AssetsModel.GetBalance(Model.AjunaClientModel.GroupClients, Model.AjunaClientModel.GroupEndpoints, KeysModel.GetSubstrateKey());

        usdBalanceViewModel.UpdateBalances();
    }
}

