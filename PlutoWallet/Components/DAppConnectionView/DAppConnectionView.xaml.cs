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
        var manager = Model.PlutonicationModel.EventManager;
        manager.StopReceiveLoop();
        manager.CloseConnection();

        Model.PlutonicationModel.EventManager = null;

        var viewModel = DependencyService.Get<DAppConnectionViewModel>();
        viewModel.IsVisible = false;
    }
}
