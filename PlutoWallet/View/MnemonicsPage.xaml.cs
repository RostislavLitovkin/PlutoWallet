namespace PlutoWallet.View;

public partial class MnemonicsPage : ContentPage
{
	public MnemonicsPage()
	{
		InitializeComponent();
	}

	private async void GoToEnterMnemonics(System.Object sender, System.EventArgs e)
    {
		await Shell.Current.GoToAsync("EnterMnemonics");
    }

    private async void ContinueToMainPageClicked(System.Object sender, System.EventArgs e)
	{
		viewModel.Continue();
        await Shell.Current.GoToAsync("MainPage");
    }
}
