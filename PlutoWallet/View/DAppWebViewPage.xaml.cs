namespace PlutoWallet.View;

public partial class DAppWebViewPage : ContentPage
{
	public DAppWebViewPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

    }
}
