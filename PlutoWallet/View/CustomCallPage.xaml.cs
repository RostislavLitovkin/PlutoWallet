namespace PlutoWallet.View;

public partial class CustomCallPage : ContentPage
{
    //private ViewModel.CustomCallsViewModel bind;
    public CustomCallPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        BindingContext = DependencyService.Get<ViewModel.CustomCallsViewModel>();

        InitializeComponent();
 
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //await viewModel.GetMetadataAsync();
    }

    private async void SubmitClicked(System.Object sender, System.EventArgs e)
    {
        var thing = DependencyService.Get<ViewModel.CustomCallsViewModel>();
        await thing.SubmitCallAsync();
    }
}
