using PlutoWallet.View;
using PlutoWallet.ViewModel;
using PlutoWallet.Components.TransferView;

namespace PlutoWallet.Components.NavigationBar;

public partial class NavigationBarView : ContentView
{
	public NavigationBarView()
	{
		InitializeComponent();
	}

    void OnHomeClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BasePageViewModel>();

        viewModel.Content = new MainView();

        nftsSpan.FontAttributes = FontAttributes.None;
        homeSpan.FontAttributes = FontAttributes.Bold;
    }

    void OnNFTsClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BasePageViewModel>();

        viewModel.Content = new NftView();

        nftsSpan.FontAttributes = FontAttributes.Bold;
        homeSpan.FontAttributes = FontAttributes.None;
    }
    
    void OnTransferClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<TransferViewModel>();

        viewModel.IsVisible = true;
    }
}
