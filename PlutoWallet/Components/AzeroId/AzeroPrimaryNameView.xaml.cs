namespace PlutoWallet.Components.AzeroId;

public partial class AzeroPrimaryNameView : ContentView
{
	public AzeroPrimaryNameView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<AzeroPrimaryNameViewModel>();
    }
}
