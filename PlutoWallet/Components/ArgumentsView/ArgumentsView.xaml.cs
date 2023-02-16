namespace PlutoWallet.Components.ArgumentsView;

public partial class ArgumentsView : ContentView
{
	public ArgumentsView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ArgumentsViewModel>();
    }
}
