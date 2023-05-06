namespace PlutoWallet.Components.Balance;

public partial class UsdBalanceView : ContentView
{
	public UsdBalanceView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<UsdBalanceViewModel>();
    }
}
