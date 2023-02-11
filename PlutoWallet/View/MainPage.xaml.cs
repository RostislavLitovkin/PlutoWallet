using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<MainViewModel>();
    }

    async void TransferClicked(System.Object sender, System.EventArgs e)
    {
        ((Button)sender).IsEnabled = false;

        transferView.IsVisible = true;
        await transferView.FadeTo(1, 500);

        ((Button)sender).IsEnabled = true;
    }

    async void ReloadClicked(System.Object sender, System.EventArgs e)
    {
        ((Button)sender).IsEnabled = false;

        var viewModel = DependencyService.Get<MainViewModel>();
        await viewModel.GetBalanceAsync();

        ((Button)sender).IsEnabled = true;

    }

    async void OnQRClicked(System.Object sender, System.EventArgs e)
    {
       

        //await universalScannerView.Appear();

        return;


        //connectionRequestView.DAppName = "internal dApp";
        //connectionRequestView.IconUrl = "internal dApp";

    }

    async void OnSettingsClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

