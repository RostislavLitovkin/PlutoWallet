using PlutoWallet.View;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.TransferView;

namespace PlutoWallet.Components.NavigationBar;

public partial class NavigationBarView : ContentView
{
	public NavigationBarView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<NavigationBarViewModel>();
    }

    void OnHomeClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BasePageViewModel>();

        viewModel.SetMainView();
    }

    void OnNFTsClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BasePageViewModel>();

        viewModel.SetNftView();
    }
    
    async void OnTransferClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<TransferViewModel>();

        viewModel.IsVisible = true;

        viewModel.GetFeeAsync();
    }
}
