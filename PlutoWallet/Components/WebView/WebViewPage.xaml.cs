namespace PlutoWallet.Components.WebView;

public partial class WebViewPage : ContentPage
{
	public WebViewPage(string url)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        webView.Source = url;
        topNavigationBar.Title = url;
    }
}
