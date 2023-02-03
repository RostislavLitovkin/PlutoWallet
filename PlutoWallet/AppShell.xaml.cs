using PlutoWallet.View;

namespace PlutoWallet;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("GenerateMnemonics", typeof(MnemonicsPage));
        Routing.RegisterRoute("EnterMnemonics", typeof(EnterMnemonicsPage));
        Routing.RegisterRoute("MainPage", typeof(MainPage));
    }
}
