using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Components.Staking;

public partial class StakingEntryView : ContentView
{
	public StakingEntryView()
	{
		InitializeComponent();

        easyProSwitch.FirstMethod = ShowEasyStaking;
        easyProSwitch.SecondMethod = ShowProStaking;
    }

    public bool ShowEasyStaking()
    {
        easyStaking.IsVisible = true;
        proStaking.IsVisible = false;

        this.HeightRequest = 95;

        return true;
    }

    public bool ShowProStaking()
    {
        easyStaking.IsVisible = false;
        proStaking.IsVisible = true;

        this.HeightRequest = 635;

        return true;
    }

    void OnInfoClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var infoViewModel = DependencyService.Get<MessagePopupViewModel>();

        infoViewModel.Title = "One click staking";
        infoViewModel.Text = "This is the most simple way to start staking on Polkadot ecosystem.\n\n" +
            "Do not know what the staking is?\n" +
            "It is a simple way to start earning a passive income while helping to secure the Polkadot blockchains.\n\n" +
            "Just tap the Stake button, sign a simple registration transaction and start earning now.";

        infoViewModel.IsVisible = true;
    }

    void OnStakeClicked(System.Object sender, System.EventArgs e)
    {
        var stakingRegistrationViewModel = DependencyService.Get<StakingRegistrationRequestViewModel>();

        // Add more info later

        stakingRegistrationViewModel.IsVisible = true;
    }
}
