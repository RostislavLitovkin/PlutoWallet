using PlutoWallet.Components.WebView;

namespace PlutoWallet.Components.Staking;

public partial class StakingDashboardView : ContentView
{
	public StakingDashboardView()
	{
        InitializeComponent();

        BindingContext = DependencyService.Get<StakingDashboardViewModel>();

        easyProSwitch.FirstMethod = ShowEasyStaking;
        easyProSwitch.SecondMethod = ShowProStaking;
    }

    public bool ShowEasyStaking()
    {
        var viewModel = DependencyService.Get<StakingDashboardViewModel>();

        viewModel.Content = new StakingEntryView();

        return true;
    }

    public bool ShowProStaking()
    {
        var viewModel = DependencyService.Get<StakingDashboardViewModel>();

        viewModel.Content = new AdvancedWebView { Address = "https://staking.polkadot.network/#/overview" };

        return true;
    }
}
