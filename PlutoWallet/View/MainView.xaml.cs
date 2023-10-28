using PlutoWallet.ViewModel;

namespace PlutoWallet.View;

public partial class MainView : ContentView
{
	public MainView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<MainViewModel>();

        Setup();
    }

	public void Setup()
	{
		if (stackLayout.Children.Count() != 0)
		{
            stackLayout.Children.Clear();
        }

		List<IView> views;
        try
		{
			views = Model.CustomLayoutModel.ParsePlutoLayout(Preferences.Get(
				"PlutoLayout",
				Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT));
		}
		catch
		{
            views = Model.CustomLayoutModel.ParsePlutoLayout(Model.CustomLayoutModel.DEFAULT_PLUTO_LAYOUT);
        }

		foreach (IView view in views)
		{
			((ContentView)view).Parent = null;
			stackLayout.Children.Add(view);
		}
	}
}
