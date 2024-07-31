using PlutoWallet.View;

namespace PlutoWallet.Components.Settings;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    async void OnPredefinedLayoutsClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new PredefinedLayoutsPage());
    }

    async void OnLogOutClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Preferences.Remove("publicKey");
        SecureStorage.Default.Remove("privateKey");
        SecureStorage.Default.Remove("mnemonics");
        SecureStorage.Default.Remove("password");
        Preferences.Remove("biometricsEnabled");

        Navigation.InsertPageBefore(new SetupPasswordPage(), Navigation.NavigationStack[0]);

        await Navigation.PopToRootAsync();
    }

    async void OnDeveloperSettingsClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new DeveloperSettingsPage());
    }

    async void OnShowMnemonicsClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        if ((await Model.KeysModel.GetMnemonicsOrPrivateKeyAsync()).IsSome(out (string, bool) secretValues))
        {
            await Navigation.PushAsync(new MnemonicsPage(secretValues));
        }
    }
}