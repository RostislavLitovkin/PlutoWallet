namespace PlutoWallet.Components.Extrinsic;

public partial class CallDetailPage : ContentPage
{
	public CallDetailPage(CallDetailViewModel viewModel)
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

		BindingContext = viewModel;
	}
}