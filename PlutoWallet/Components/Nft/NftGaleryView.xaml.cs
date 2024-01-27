using PlutoWallet.ViewModel;

namespace PlutoWallet.Components.Nft;

public partial class NftGaleryView : ContentView
{
	public NftGaleryView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<NftGaleryViewModel>();
    }

    void OnPlusClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        var viewModel = DependencyService.Get<BasePageViewModel>();

        viewModel.SetNftView();
    }
}
