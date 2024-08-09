using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Model;
using PlutoWallet.Model.AjunaExt;
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
        Console.WriteLine
            ("Reload clicked");
        var usdBalanceViewModel = DependencyService.Get<UsdBalanceViewModel>();

        usdBalanceViewModel.UsdSum = "Loading";

        usdBalanceViewModel.ReloadIsVisible = false;

        Console.WriteLine
    ("Reload ...");
        foreach (var client in Model.AjunaClientModel.Clients.Values)
        {
            await Model.AssetsModel.GetBalanceAsync(await client.Task, KeysModel.GetSubstrateKey());

            usdBalanceViewModel.UpdateBalances();
        }


        usdBalanceViewModel.ReloadIsVisible = true;
        Console.WriteLine
    ("Reload Done");
    }
}

