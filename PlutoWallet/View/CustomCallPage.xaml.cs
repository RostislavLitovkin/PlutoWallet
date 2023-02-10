namespace PlutoWallet.View;

public partial class CustomCallPage : ContentPage
{
    //private ViewModel.CustomCallsViewModel bind;
    public CustomCallPage()
	{
		InitializeComponent();
 
        BindingContext = DependencyService.Get<ViewModel.CustomCallsViewModel>();
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
