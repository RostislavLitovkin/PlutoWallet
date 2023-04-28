using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class MainView : ContentView
{
	public MainView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<MainViewModel>();
    }
}
