namespace PlutoWallet.View;

public partial class EnterMnemonicsPage : ContentPage
{
	public EnterMnemonicsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
    {
        await viewModel.CreateKeys();
        await Navigation.PushAsync(new BasePage());
    }

    private async void ContinueWithPrivateKeyClicked(System.Object sender, System.EventArgs e)
    {
        await viewModel.CreateKeysWithPrivateKey();
        await Navigation.PushAsync(new BasePage());
    }
}
