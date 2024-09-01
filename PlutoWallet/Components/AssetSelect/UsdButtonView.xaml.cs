namespace PlutoWallet.Components.AssetSelect;

public partial class UsdButtonView : ContentView
{
	public UsdButtonView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<AssetSelectButtonViewModel>();
    }

}