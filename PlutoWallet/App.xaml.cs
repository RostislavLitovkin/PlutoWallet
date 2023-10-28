using PlutoWallet.Components.ArgumentsView;
using PlutoWallet.Components.Balance;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.NetworkSelect;
using PlutoWallet.Components.PublicKeyQRCodeView;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.Components.TransferView;
using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.CalamarView;
using PlutoWallet.Components.Extrinsic;
using PlutoWallet.View;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.Staking;
using PlutoWallet.Components.CustomLayouts;
using PlutoWallet.Components.ConfirmTransaction;
using PlutoWallet.Components.AzeroId;
using PlutoWallet.Components.AssetSelect;
using PlutoWallet.Components.HydraDX;
using PlutoWallet.Components.Nft;
using PlutoWallet.Components.Identity;
using PlutoWallet.Components.Vault;
using PlutoWallet.Components.Referenda;
using PlutoWallet.Components.ChangeLayoutRequest;

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

        DependencyService.Register<MessagePopupViewModel>();

        DependencyService.Register<TransactionRequestViewModel>();

        DependencyService.Register<ArgumentsViewModel>();

        DependencyService.Register<PublicKeyQRCodeViewModel>();

        DependencyService.Register<DAppConnectionViewModel>();

        DependencyService.Register<StakingRegistrationRequestViewModel>();

        DependencyService.Register<MultiNetworkSelectViewModel>();

        DependencyService.Register<ChainAddressViewModel>();

        DependencyService.Register<BasePageViewModel>();

        DependencyService.Register<StakingDashboardViewModel>();

        DependencyService.Register<UsdBalanceViewModel>();

        DependencyService.Register<CalamarViewModel>();

        DependencyService.Register<ExtrinsicStatusStackViewModel>();

        DependencyService.Register<ExportPlutoLayoutQRViewModel>();

        DependencyService.Register<CustomItemViewModel>();

        DependencyService.Register<ConfirmTransactionViewModel>();

        DependencyService.Register<MessageSignRequestViewModel>();

        DependencyService.Register<NftViewModel>();

        DependencyService.Register<AzeroPrimaryNameViewModel>();

        DependencyService.Register<AssetSelectViewModel>();

        DependencyService.Register<AssetSelectButtonViewModel>();

        DependencyService.Register<OmnipoolLiquidityViewModel>();

        DependencyService.Register<DCAViewModel>();

        DependencyService.Register<NftLoadingViewModel>();

        DependencyService.Register<IdentityViewModel>();

        DependencyService.Register<VaultSignViewModel>();

        DependencyService.Register<ReferendaViewModel>();

        DependencyService.Register<ChangeLayoutRequestViewModel>();

        DependencyService.Register<NetworkSelectPopupViewModel>();

        if ((Preferences.ContainsKey("mnemonics") && "" != Preferences.Get("mnemonics", "")) || (Preferences.ContainsKey("privateKey") && "" != Preferences.Get("privateKey", "")))
        {
            MainPage = new NavigationPage(new BasePage());
        }
        else
        {
            MainPage = new NavigationPage(new BeginPage());
        }
	}
}
