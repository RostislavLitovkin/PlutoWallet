using PlutoWallet.Components.MessagePopup;
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
        await Model.AssetsModel.GetBalance();
    }
}

