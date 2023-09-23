using PlutoWallet.Components.Card;

namespace PlutoWallet.Components.AssetSelect;

public partial class AssetSelectView : ContentView
{
	public AssetSelectView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<AssetSelectViewModel>();
    }
}
