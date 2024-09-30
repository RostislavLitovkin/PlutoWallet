namespace PlutoWallet.Components.MessagePopup;

public partial class MessagePopupView : ContentView
{
	public MessagePopupView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<MessagePopupViewModel>();
    }


    async void OnBackTapped(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        // Hide this layout
        //await this.FadeTo(0, 500);
        var viewModel = DependencyService.Get<MessagePopupViewModel>();
        viewModel.IsVisible = false;
    }

    async void OnBackClicked(System.Object sender, System.EventArgs e)
    {
        // Hide this layout
        //await this.FadeTo(0, 500);
        var viewModel = DependencyService.Get<MessagePopupViewModel>();
        viewModel.IsVisible = false;
    }
}
