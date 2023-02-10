namespace PlutoWallet.View;

public partial class MnemonicsPage : ContentPage
{
	public MnemonicsPage()
	{
		InitializeComponent();
	}

	private async void GoToEnterMnemonics(System.Object sender, System.EventArgs e)
    {
		await Navigation.PushAsync(new EnterMnemonicsPage());
    }

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
	{
        viewModel.Continue();
		await Navigation.PushAsync(new MainTabbedPage());
    }
}
