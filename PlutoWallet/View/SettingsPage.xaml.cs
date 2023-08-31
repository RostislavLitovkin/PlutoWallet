namespace PlutoWallet.View;
using CommunityToolkit.Maui.Alerts;

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
        Preferences.Set("privateKey", "");
        Preferences.Set("publicKey", "");
        Preferences.Set("mnemonics", "");
        Preferences.Set("password", "");
        Preferences.Set("biometricsEnabled", false);


        Preferences.Clear("privateKey");
        Preferences.Clear("publicKey");
        Preferences.Clear("mnemonics");
        Preferences.Clear("password");
        Preferences.Clear("biometricsEnabled");

        Console.WriteLine("Private key deleted");

        await Navigation.PushAsync(new BeginPage());

        for (int i = 0; i < Navigation.NavigationStack.Count() - 1; i++)
        {
            Navigation.RemovePage(Navigation.NavigationStack[i]);
        }
    }
}
