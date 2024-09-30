using Plutonication;

#if ANDROID29_0_OR_GREATER
using PlutoWallet.Platforms.Android;
using Android.Content;
#endif

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

#if ANDROID29_0_OR_GREATER
            var mainActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            Intent serviceIntent = new Intent(mainActivity, typeof(PlutonicationAndroidForegroundService));

            mainActivity.StopService(serviceIntent);
#endif
        }
        catch
        {
            // Fails if it is not connected to anything. (Which is fine)
        }

        var viewModel = DependencyService.Get<DAppConnectionViewModel>();
        viewModel.IsVisible = false;
    }
}
