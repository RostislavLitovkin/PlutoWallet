namespace PlutoWallet.Components.ChangeLayoutRequest;

public partial class ChangeLayoutRequestView : ContentView
{
	public ChangeLayoutRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ChangeLayoutRequestViewModel>();
    }

    async void OnAddClicked(System.Object sender, System.EventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<ChangeLayoutRequestViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnRejectClicked(System.Object sender, System.EventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<ChangeLayoutRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
