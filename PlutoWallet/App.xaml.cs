using PlutoWallet.Components.ArgumentsView;
using PlutoWallet.Components.BalanceView;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.PublicKeyQRCodeView;
using PlutoWallet.Components.ScannerView;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Components.TransferView;
using PlutoWallet.Components.DAppConnectionView;
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

        DependencyService.Register<ConnectionRequestViewModel>();

        DependencyService.Register<ScannerViewModel>();

        DependencyService.Register<MessagePopupViewModel>();

        DependencyService.Register<TransactionRequestViewModel>();

        DependencyService.Register<BalanceViewModel>();

        DependencyService.Register<ArgumentsViewModel>();

        DependencyService.Register<PublicKeyQRCodeViewModel>();

        DependencyService.Register<DAppConnectionViewModel>();

        if (Preferences.ContainsKey("privateKey"))
        {
            MainPage = new NavigationPage(new MainTabbedPage());
        }
        else
        {
            MainPage = new NavigationPage(new MnemonicsPage());
        }
	}
}
