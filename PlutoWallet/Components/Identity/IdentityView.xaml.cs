namespace PlutoWallet.Components.Identity;

public partial class IdentityView : ContentView
{
	public IdentityView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<IdentityViewModel>();
    }
}
