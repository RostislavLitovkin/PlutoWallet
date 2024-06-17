using Plutonication;
using PlutoWallet.Components.MessagePopup;

namespace PlutoWallet.Components.DAppConnectionView;

public partial class DAppConnectionView : ContentView
{
	public DAppConnectionView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<DAppConnectionViewModel>();
    }

    public DAppConnectionView(DAppConnectionViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    async void OnDisconnectClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        try
        {
            // Disconnect from the dApp
            await PlutonicationWalletClient.DisconnectAsync();
        }
        catch
        {
            // Fails if it is not connected to anything. (Which is fine)
        }

        var viewModel = DependencyService.Get<DAppConnectionViewModel>();
        viewModel.IsVisible = false;
    }
}
