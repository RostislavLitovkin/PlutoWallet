using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Components.DAppConnectionView;

public partial class DAppConnectionView : ContentView
{
	public DAppConnectionView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<DAppConnectionViewModel>();
    }

    void OnDisconnectClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            var manager = Model.PlutonicationModel.EventManager;
            manager.StopReceiveLoop();
            manager.CloseConnection();

            Model.PlutonicationModel.EventManager = null;

            var viewModel = DependencyService.Get<DAppConnectionViewModel>();
            viewModel.IsVisible = false;
        }
        catch (Exception ex)
        {
            var messagePopup = DependencyService.Get<MessagePopupViewModel>();

            messagePopup.Title = "Error";
            messagePopup.Text = ex.Message;

            messagePopup.IsVisible = true;
        }
    }
}
