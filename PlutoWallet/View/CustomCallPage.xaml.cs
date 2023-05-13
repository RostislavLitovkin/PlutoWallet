namespace PlutoWallet.View;

public partial class CustomCallPage : ContentView
{
    //private ViewModel.CustomCallsViewModel bind;
    public CustomCallPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = DependencyService.Get<ViewModel.CustomCallsViewModel>();

        InitializeComponent();
 
    }

    private async void SubmitClicked(System.Object sender, System.EventArgs e)
    {
        var thing = DependencyService.Get<ViewModel.CustomCallsViewModel>();
        await thing.SubmitCallAsync();
    }
}
