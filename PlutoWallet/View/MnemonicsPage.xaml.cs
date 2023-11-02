namespace PlutoWallet.View;

public partial class MnemonicsPage : ContentPage
{

    /// <summary>
    /// Intended for use with `KeysModel.GetMnemonicsOrPrivateKeyAsync()`
    /// </summary>
    /// <param name="secretValues"></param>
	public MnemonicsPage((string, bool) secretValues)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        var (mnemonicsOrPrivateKey, usePrivateKey) = secretValues;

        viewModel.Title = usePrivateKey ? "Private key:" : "Mnemonics:";

        viewModel.Mnemonics = mnemonicsOrPrivateKey;
	}

	private async void GoToEnterMnemonics(System.Object sender, System.EventArgs e)
    {
		await Navigation.PushAsync(new EnterMnemonicsPage());
    }

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PopToRootAsync();
    }
}
