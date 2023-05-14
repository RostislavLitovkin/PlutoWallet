namespace PlutoWallet.Components.Extrinsic;

public partial class ExtrinsicStatusStackLayout : ContentView
{
	public ExtrinsicStatusStackLayout()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ExtrinsicStatusStackViewModel>();
    }
}
