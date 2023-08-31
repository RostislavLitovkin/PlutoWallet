using Schnorrkel.Keys;
using Substrate.NetApi;
using static Substrate.NetApi.Mnemonic;

namespace PlutoWallet.View;

public partial class BeginPage : ContentPage
{
	public BeginPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
    }

    async void BeginClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new MnemonicsPage());
    }
}
