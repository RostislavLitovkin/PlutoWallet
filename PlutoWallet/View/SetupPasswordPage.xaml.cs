namespace PlutoWallet.View;

public partial class SetupPasswordPage : ContentPage
{
	public SetupPasswordPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
    {
        await Model.KeysModel.GenerateNewAccountAsync(passwordEntry.Text);

        Navigation.InsertPageBefore(new BasePage(), Navigation.NavigationStack[0]);
        await Navigation.PopToRootAsync();
    }
}
