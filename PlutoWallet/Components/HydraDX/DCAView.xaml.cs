namespace PlutoWallet.Components.HydraDX;

public partial class DCAView : ContentView
{
	public DCAView()
	{
		InitializeComponent();
        BindingContext = DependencyService.Get<DCAViewModel>();
    }
}
