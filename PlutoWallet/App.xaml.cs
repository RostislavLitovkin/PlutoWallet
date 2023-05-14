using PlutoWallet.Components.ArgumentsView;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.PublicKeyQRCodeView;
using PlutoWallet.Components.ScannerView;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Components.TransferView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.CalamarView;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.View;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.Staking;

namespace PlutoWallet;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

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

        DependencyService.Register<StakingRegistrationRequestViewModel>();

        DependencyService.Register<MultiNetworkSelectViewModel>();

        DependencyService.Register<ChainAddressViewModel>();

        DependencyService.Register<BasePageViewModel>();

        DependencyService.Register<StakingDashboardViewModel>();

        DependencyService.Register<BalanceDashboardViewModel>();

        DependencyService.Register<UsdBalanceViewModel>();

        DependencyService.Register<CalamarViewModel>();

        DependencyService.Register<ExtrinsicStatusStackViewModel>();

        if (Preferences.ContainsKey("privateKey"))
        {
            MainPage = new NavigationPage(new BasePage());
        }
        else
        {
            MainPage = new NavigationPage(new MnemonicsPage());
        }
	}
}
