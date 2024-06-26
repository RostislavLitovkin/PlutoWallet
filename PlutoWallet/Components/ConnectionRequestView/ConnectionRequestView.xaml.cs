using PlutoWallet.Components.DAppConnectionView;
using PlutoWallet.Model;

#if ANDROID26_0_OR_GREATER
using PlutoWallet.Platforms.Android;
using Android.Content;
#endif

namespace PlutoWallet.Components.ConnectionRequestView;

public partial class ConnectionRequestView : ContentView
{
	public ConnectionRequestView()
	{
		InitializeComponent();

        BindingContext = DependencyService.Get<ConnectionRequestViewModel>();
    }

    private async void AcceptClicked(System.Object sender, System.EventArgs e)
    {
#if ANDROID26_0_OR_GREATER
        var mainActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
        Intent serviceIntent = new Intent(mainActivity, typeof(PlutonicationAndroidForegroundService));

        mainActivity.StartForegroundService(serviceIntent);
#else
        await PlutonicationModel.AcceptConnectionAsync();
#endif
    }

    private void RejectClicked(System.Object sender, System.EventArgs e)
    {
        DAppConnectionViewModel dAppViewModel = DependencyService.Get<DAppConnectionViewModel>();
        dAppViewModel.SetConnectionState(DAppConnectionStateEnum.Rejected);

        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }

    private void DismissClicked(System.Object sender, System.EventArgs e)
    {
        var viewModel = DependencyService.Get<ConnectionRequestViewModel>();
        viewModel.IsVisible = false;
    }
}
