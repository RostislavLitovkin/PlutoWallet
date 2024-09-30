namespace PlutoWallet.Components.Staking;

public partial class StakingRegistrationRequestView : ContentView
{
	public StakingRegistrationRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<StakingRegistrationRequestViewModel>();
    }

    async void OnBackClicked(System.Object sender, System.EventArgs e)
    {
        // Hide this layout
        //await this.FadeTo(0, 500);
        var viewModel = DependencyService.Get<StakingRegistrationRequestViewModel>();
        viewModel.IsVisible = false;
    }

    void OnSubmitClicked(System.Object sender, System.EventArgs e)
    {
    }

    void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
    }
}
