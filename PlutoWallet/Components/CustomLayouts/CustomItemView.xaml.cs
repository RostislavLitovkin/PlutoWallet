namespace PlutoWallet.Components.CustomLayouts;

public partial class CustomItemView : ContentView
{

    public CustomItemView()
	{
		InitializeComponent();
        
        BindingContext = DependencyService.Get<CustomItemViewModel>();
    }

    async void OnBackClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        var viewModel = DependencyService.Get<CustomItemViewModel>();

        viewModel.IsVisible = false;
    }
}
