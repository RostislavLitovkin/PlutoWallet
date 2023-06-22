namespace PlutoWallet.View;

public partial class PredefinedLayoutsPage : ContentPage
{
	public PredefinedLayoutsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    private async void OnPlusClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new CustomLayoutsPage());
    }
}
