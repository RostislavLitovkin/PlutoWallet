using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class BasePage : ContentPage
{
	public BasePage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        BindingContext = DependencyService.Get<BasePageViewModel>();
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
