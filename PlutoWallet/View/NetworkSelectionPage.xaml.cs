using PlutoWallet.ViewModel;
using PlutoWallet.Constants;
using PlutoWallet.Components.NetworkSelect;

namespace PlutoWallet.View;

public partial class NetworkSelectionPage : ContentPage
{
	public NetworkSelectionPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
    }
}
