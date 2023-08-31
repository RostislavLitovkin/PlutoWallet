namespace PlutoWallet.View;

public partial class RegisterBiometricsPage : ContentPage
{
	public RegisterBiometricsPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
	}

    void OnUseBiometricsClicked(System.Object sender, System.EventArgs e)
    {

    }

    void OnRejectClicked(System.Object sender, System.EventArgs e)
    {

    }
}
