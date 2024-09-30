using PlutoWallet.View;

namespace PlutoWallet.Components.Settings;

public partial class DeveloperSettingsPage : ContentPage
{
	public DeveloperSettingsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void OnCreateCustomLayoutsClicked(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new CustomLayoutsPage());
    }
}