namespace PlutoWallet.View;

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
}
