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
        await Model.KeysModel.GenerateNewAccountAsync(passwordEntry.Text != null ? passwordEntry.Text : "");

        Console.WriteLine("Account created");

        Navigation.InsertPageBefore(new BasePage(), Navigation.NavigationStack[0]);
        await Navigation.PopToRootAsync();
    }

    private void OnEyeballClicked(object sender, TappedEventArgs e)
    {
        passwordEntry.IsPassword = !passwordEntry.IsPassword;

        eyeball.IsVisible = passwordEntry.IsPassword;
        eyeballSlash.IsVisible = !passwordEntry.IsPassword;
    }

    private async void OnEnterPressedAsync(object sender, EventArgs e)
    {
        var entry = (Entry)sender;
        if (entry.IsSoftInputShowing())
            await entry.HideSoftInputAsync(System.Threading.CancellationToken.None);
    }
}
