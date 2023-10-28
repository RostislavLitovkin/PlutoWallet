namespace PlutoWallet.Components.HydraDX;

public partial class OmnipoolLiquidityView : ContentView
{
	public OmnipoolLiquidityView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<OmnipoolLiquidityViewModel>();
    }
}
