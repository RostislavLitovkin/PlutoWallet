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

    void OnLogOutClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Preferences.Set("privateKey", "");
        Preferences.Clear("privateKey");


        Console.WriteLine("Private key deleted");
    }
}
