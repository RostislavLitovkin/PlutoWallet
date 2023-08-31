namespace PlutoWallet.Components.Kodadot;

public partial class KodadotUnlockablePage : ContentPage
{
	public KodadotUnlockablePage(string url)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

		webView.Source = url;
		topNavigationBar.Title = url;
    }
}
