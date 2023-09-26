using PlutoWallet.Components.MessagePopup;
using PlutoWallet.NetApiExt.Generated.Model.pallet_assets.types;

namespace PlutoWallet.Components.Balance;

public partial class UsdBalanceView : ContentView
{
	public UsdBalanceView()
	{
		InitializeComponent();

        var viewModel = DependencyService.Get<UsdBalanceViewModel>();

        viewModel.ReloadBalanceViewStackLayout = ReloadBalanceViewStackLayout;

        BindingContext = viewModel;
    }

    async void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Model.AssetsModel.GetBalance();
    }

    public void ReloadBalanceViewStackLayout(List<AssetAmountView> assets)
    {
        for (int i = 0; i < balanceViewStackLayout.Children.Count(); i++)
        {
            ((AssetAmountView)balanceViewStackLayout.Children[i]).IsVisible = false;
        }

        int count = assets.Count() <= balanceViewStackLayout.Children.Count() ? assets.Count() : balanceViewStackLayout.Children.Count();

        for (int i = 0; i < count; i++)
        {
            ((AssetAmountView)balanceViewStackLayout.Children[i]).IsVisible = true;
            ((AssetAmountView)balanceViewStackLayout.Children[i]).Amount = assets[i].Amount;
            ((AssetAmountView)balanceViewStackLayout.Children[i]).Symbol = assets[i].Symbol;
            ((AssetAmountView)balanceViewStackLayout.Children[i]).UsdValue = assets[i].UsdValue;
            ((AssetAmountView)balanceViewStackLayout.Children[i]).ChainIcon = assets[i].ChainIcon;
        }
    }
}

