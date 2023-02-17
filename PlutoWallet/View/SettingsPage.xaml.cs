namespace PlutoWallet.View;
using CommunityToolkit.Maui.Alerts;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

    //private void SaveEndpoint(System.Object sender, System.EventArgs e)
    //{
    //    viewModel.SaveEndpoint();
    //}

    private void ClearEndpoints(System.Object sender, System.EventArgs e)
    {
        viewModel.ClearEndpoints();
    }

    private void ShowPrivateKey(System.Object sender, System.EventArgs e)
    {
        viewModel.ShowPrivateKey();
        showKey.Text = Preferences.Get("privateKey", "");
        copyKey.IsVisible = true;
    }

    private async void CopyText(System.Object sender, System.EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(Preferences.Get("privateKey", ""));
        var toast = Toast.Make("Copied to clipboard");
        await toast.Show();
    }
}
