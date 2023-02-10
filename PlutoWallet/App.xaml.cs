using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.TransferView;
using PlutoWallet.View;
using PlutoWallet.ViewModel;

namespace PlutoWallet;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        DependencyService.Register<NetworkSelectViewModel>();

        DependencyService.Register<CustomCallsViewModel>();

        DependencyService.Register<MainViewModel>();

        DependencyService.Register<TransferViewModel>();


        if (Preferences.ContainsKey("privateKey") && false)
        {
            MainPage = new NavigationPage(new MainTabbedPage());
        }
        else
        {
            MainPage = new NavigationPage(new MnemonicsPage());
        }
	}
}
