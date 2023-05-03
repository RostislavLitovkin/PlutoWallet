using PlutoWallet.Components.WebView;

namespace PlutoWallet.Components.Balance;

public partial class BalanceDashboardView : ContentView
{
	public BalanceDashboardView()
	{
        BindingContext = DependencyService.Get<BalanceDashboardViewModel>();
        InitializeComponent();

        valueGraphSwitch.FirstMethod = ShowValue;
        valueGraphSwitch.SecondMethod = ShowGraph;
    }

    public bool ShowValue()
    {
        var viewModel = DependencyService.Get<BalanceDashboardViewModel>();
        viewModel.Content = new UsdBalanceView();

        return true;
    }

    public bool ShowGraph()
    {
        var viewModel = DependencyService.Get<BalanceDashboardViewModel>();
        viewModel.Content = new AdvancedWebView { Address = "https://www.coingecko.com/en/coins/polkadot" };

        return true;
    }
}
