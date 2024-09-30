namespace PlutoWallet.Components.UpdateView;

public partial class UpdateView : ContentView
{
	public UpdateView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<UpdateViewModel>();
    }

    async void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            await Launcher.Default.OpenAsync("https://play.google.com/store/apps/details?id=com.rostislavlitovkin.plutowallet");
        }
        catch
        {

        }
    }
}
