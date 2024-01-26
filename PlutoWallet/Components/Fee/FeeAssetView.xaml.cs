namespace PlutoWallet.Components.Fee;

public partial class FeeAssetView : ContentView
{
	public FeeAssetView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<FeeAssetViewModel>();
    }
}
