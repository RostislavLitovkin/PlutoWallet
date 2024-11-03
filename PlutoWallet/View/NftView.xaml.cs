using PlutoWallet.Components.Nft;
using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class NftView : ContentView
{
	public NftView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<NftViewModel>();
    }

    private async void ShowAllNftsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NftListPage(new NftListOwnedViewModel()));
    }
}
