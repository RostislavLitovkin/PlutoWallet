using System.Net;
using Ajuna.NetApi.Model.Extrinsics;
using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.TransactionRequest;
using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class MainTabbedPage : TabbedPage
{
	public MainTabbedPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();

	}

}
