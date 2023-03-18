using PlutoWallet.ViewModel;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.TransactionRequest;

using Ajuna.NetApi.Model.Extrinsics;
using Plutonication;

namespace PlutoWallet.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = DependencyService.Get<MainViewModel>();

        InitializeComponent();
    }

    async void OnQRClicked(System.Object sender, System.EventArgs e)
    {
        await universalScannerView.Appear();
    }

    async void OnSettingsClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

