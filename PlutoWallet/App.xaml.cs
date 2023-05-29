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
using PlutoWallet.Components.CustomLayouts;

namespace PlutoWallet;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        RegisterDependencies();

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
