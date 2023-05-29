namespace PlutoWallet.Components.NavigationBar;

public partial class TopNavigationBar : ContentView
{
	public TopNavigationBar()
	{
		InitializeComponent();
	}

    public string Title { set { titleText.Text = value; } }

    public string ExtraTitle { set { extraLabel.Text = value; } }

    public Func<Task> ExtraFunc { get; set; }

    private async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		await Navigation.PopAsync();
    }

    private async void OnExtraClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await ExtraFunc();
    }
}
