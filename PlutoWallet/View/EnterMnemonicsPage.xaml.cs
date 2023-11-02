namespace PlutoWallet.View;

public partial class EnterMnemonicsPage : ContentPage
{
	public EnterMnemonicsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void ContinueWithMnemonicClicked(System.Object sender, System.EventArgs e)
    {
        await Model.KeysModel.GenerateNewAccountAsync(
            viewModel.Mnemonics,
            await SecureStorage.Default.GetAsync("password")
        );

        await Navigation.PopToRootAsync();
    }

    private async void ContinueWithPrivateKeyClicked(System.Object sender, System.EventArgs e)
    {
        await Model.KeysModel.GenerateNewAccountFromPrivateKeyAsync(viewModel.PrivateKey);

        await Navigation.PopToRootAsync();
    }
}
