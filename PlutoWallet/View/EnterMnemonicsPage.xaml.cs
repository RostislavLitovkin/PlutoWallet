namespace PlutoWallet.View;

public partial class EnterMnemonicsPage : ContentPage
{
	public EnterMnemonicsPage()
	{
		InitializeComponent();
	}
    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
    {
        viewModel.CreateKeys();
        await Navigation.PushAsync(new BasePage());
    }
}
