namespace PlutoWallet.Components.VTokens;

public partial class VDotTokenView : ContentView
{
	public VDotTokenView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<VDotTokenViewModel>();
    }
}
