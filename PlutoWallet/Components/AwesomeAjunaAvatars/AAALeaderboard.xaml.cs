namespace PlutoWallet.Components.AwesomeAjunaAvatars;

public partial class AAALeaderboard : ContentView
{
	public AAALeaderboard()
	{
		InitializeComponent();
	}

    async void OnOpenClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Launcher.OpenAsync(webView.Address);
    }

    void OnReloadClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        webView.Address = "https://aaa.ajuna.io/leaderboard/avatars";
    }
}
