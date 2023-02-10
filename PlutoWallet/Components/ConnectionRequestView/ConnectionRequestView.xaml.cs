namespace PlutoWallet.Components.ConnectionRequestView;

public partial class ConnectionRequestView : ContentView
{
	public ConnectionRequestView()
	{
		InitializeComponent();
	}

    public string IconUrl
    {
        set
        {
            icon.Source = value;
        }
    }
    public string DAppName
    {
        set
        {
            dAppNameLabel.Text = value;
        }
    }

    void AcceptClicked(System.Object sender, System.EventArgs e)
    {

    }

    async void RejectClicked(System.Object sender, System.EventArgs e)
    {
        await this.FadeTo(0, 500);
        this.IsVisible = false;
    }
}
