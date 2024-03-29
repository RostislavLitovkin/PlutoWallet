using PlutoWallet.ViewModel;
using PlutoWallet.Components.UniversalScannerView;
using Plutonication;
using PlutoWallet.Components.MessagePopup;
using PlutoWallet.Components.ConnectionRequestView;
using PlutoWallet.Components.Vault;
using Substrate.NetApi;
using PlutoWallet.Components.TransferView;

namespace PlutoWallet.View;

public partial class BasePage : ContentPage
{
	public BasePage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();

        BindingContext = DependencyService.Get<BasePageViewModel>();
    }

    async void OnQRClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new UniversalScannerPage
        {
            OnScannedMethod = OnScanned
        });
    }

    async void OnSettingsClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    async void OnScanned(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (e.Results.Length <= 0)
            {
                return;
            }

            try
            {
                var scannedValue = e.Results[0].Value;

                // trying to connect to a dApp
                if (scannedValue.Length > 14 && scannedValue.Substring(0, 14) == "plutonication:")
                {
                    AccessCredentials ac = new AccessCredentials(new Uri(scannedValue));

                    var connectionRequest = DependencyService.Get<ConnectionRequestViewModel>();

                    connectionRequest.Show();
                    connectionRequest.Icon = ac.Icon;
                    connectionRequest.Name = ac.Name;

                    connectionRequest.Url = ac.Url;
                    connectionRequest.Key = ac.Key;
                    connectionRequest.AccessCredentials = ac;

                }
                else if (scannedValue.Length > 13 && scannedValue.Substring(0, 13) == "plutolayout: ")
                {
                    // LATER: check validity

                    Model.CustomLayoutModel.SaveLayout(scannedValue);
                }
                else if (scannedValue.Length > 10 && scannedValue.Substring(0, 10) == "substrate:")
                {
                    var viewModel = DependencyService.Get<TransferViewModel>();

                    viewModel.IsVisible = true;

                    if (scannedValue.Substring(10).IndexOf(":") != -1)
                    {
                        viewModel.Address = scannedValue.Substring(10, scannedValue.Substring(10).IndexOf(":"));
                    }
                    else
                    {
                        viewModel.Address = scannedValue.Substring(10);
                    }

                    viewModel.GetFeeAsync();
                }
                else if (Utils.Bytes2HexString(e.Results[0].Raw).IndexOf("530102") != -1)
                {
                    var vaultSign = DependencyService.Get<VaultSignViewModel>();

                    vaultSign.SignExtrinsic("0x" + Utils.Bytes2HexString(e.Results[0].Raw).Substring(Utils.Bytes2HexString(e.Results[0].Raw).IndexOf("530102") + 6));
                }
                else
                {
                    var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                    messagePopup.Title = "Unable to read QR code";
                    messagePopup.Text = "The QR code was in incorrect format.";

                    messagePopup.IsVisible = true;
                }

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {

                // Does not make much sense now...
                return;

                var messagePopup = DependencyService.Get<MessagePopupViewModel>();

                messagePopup.Title = "BasePage Error";
                messagePopup.Text = ex.Message;

                messagePopup.IsVisible = true;
            }
        });
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
    }
}
