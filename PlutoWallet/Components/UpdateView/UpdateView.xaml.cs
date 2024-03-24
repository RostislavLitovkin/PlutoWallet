namespace PlutoWallet.Components.UpdateView;

public partial class UpdateView : ContentView
{
	public UpdateView()
	{
		InitializeComponent();

		BindingContext = DependencyService.Get<UpdateViewModel>();
    }

    void OnClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {

    }
}
