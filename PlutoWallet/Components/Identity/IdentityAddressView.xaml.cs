using PlutoWallet.Model;
using PlutoWallet.Components.UniversalScannerView;
using AzeroIdResolver;

namespace PlutoWallet.Components.Identity;

public partial class IdentityAddressView : ContentView
{
    private bool isAzeroIdStyle = false;

    public static readonly BindableProperty DestinationAddressProperty = BindableProperty.Create(
        nameof(DestinationAddress), typeof(string), typeof(IdentityAddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {

        });

    // This is also just an input address
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(IdentityAddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: async(bindable, oldValue, newValue) =>
        {
            var control = (IdentityAddressView)bindable;

            control.addressEntry.Text = (string)newValue;

            // I had to duplicate these lines because of a weird error on Android.
            if (((string)newValue).Contains("."))
            {
                var newAddress = await TzeroId.GetAddressForName((string)newValue);

                Console.WriteLine(newAddress);

                if (newAddress != null)
                {
                    control.DestinationAddress = newAddress;

                    if (!control.isAzeroIdStyle)
                    {
                        control.border.BackgroundColor = Color.FromArgb("222222");

                        control.identityJundgementIcon.Source = "azeroid.png";

                        control.identityLabel.Text = newAddress;

                        control.identityLabel.TextColor = Color.FromArgb("FFFFFF");

                        control.addressEntry.TextColor = Color.FromArgb("FFFFFF");

                        control.qrcode.Source = "qrcodewhite.png";

                        control.isAzeroIdStyle = true;
                    }
                    return;
                }
            }

            if (control.isAzeroIdStyle)
            {
                control.border.SetAppThemeColor(Border.BackgroundColorProperty, Color.FromArgb("fdfdfd"), Color.FromArgb("0a0a0a"));

                control.identityLabel.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);

                control.addressEntry.SetAppThemeColor(Label.TextColorProperty, Colors.Black, Colors.White);

                control.qrcode.SetAppTheme<FileImageSource>(Image.SourceProperty, "qrcodeblack.png", "qrcodewhite.png");

                control.isAzeroIdStyle = false;
            }

            control.DestinationAddress = (string)newValue;

            var identity = await Model.IdentityModel.GetIdentityForAddress(AjunaClientModel.Client, (string)newValue);

            if (identity == null)
            {
                control.identityLabel.Text = "Unknown";
                if (Application.Current.RequestedTheme == AppTheme.Light)
                {
                    control.identityJundgementIcon.Source = "unknownblack.png";
                }
                else
                {
                    control.identityJundgementIcon.Source = "unknownwhite.png";
                }
                return;
            }

            control.identityLabel.Text = identity.DisplayName;

            switch (identity.FinalJudgement)
            {
                case Judgement.Unknown:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        control.identityJundgementIcon.Source = "unknownblack.png";
                    }
                    else
                    {
                        control.identityJundgementIcon.Source = "unknownwhite.png";
                    }
                    break;
                case Judgement.LowQuality:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        control.identityJundgementIcon.Source = "unknownblack.png";
                    }
                    else
                    {
                        control.identityJundgementIcon.Source = "unknownwhite.png";
                    }
                    break;
                case Judgement.OutOfDate:
                    if (Application.Current.RequestedTheme == AppTheme.Light)
                    {
                        control.identityJundgementIcon.Source = "unknownblack.png";
                    }
                    else
                    {
                        control.identityJundgementIcon.Source = "unknownwhite.png";
                    }
                    break;
                case Judgement.Reasonable:
                    control.identityJundgementIcon.Source = "greentick.png";
                    break;
                case Judgement.KnownGood:
                    control.identityJundgementIcon.Source = "greentick.png";
                    break;
                case Judgement.Erroneous:
                    control.identityJundgementIcon.Source = "redallert.png";
                    break;

            }
        });

    public IdentityAddressView()
	{
		InitializeComponent();
	}

    public string DestinationAddress
    {
        get => (string)GetValue(DestinationAddressProperty);

        set => SetValue(DestinationAddressProperty, value);
    }

    // This is also just an input address
    public string Address
    {
        get => (string)GetValue(AddressProperty);

        set => SetValue(AddressProperty, value);
    }

    async void OnShowQRClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new UniversalScannerPage
        {
            OnScannedMethod = OnScanned
        });
    }

    private async void OnEntryPropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        SetValue(AddressProperty, ((Entry)sender).Text);
    }

    void OnScanned(System.Object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            try
            {
                var scannedValue = e.Results[e.Results.Length - 1].Value;

                if (scannedValue.Length > 10 && scannedValue.Substring(0, 10) == "substrate:")
                {
                    if (scannedValue.Substring(10).IndexOf(":") != -1)
                    {
                        this.Address = scannedValue.Substring(10, scannedValue.Substring(10).IndexOf(":"));
                    }
                    else
                    {
                        this.Address = scannedValue.Substring(10);
                    }
                }
                else
                {
                    this.Address = scannedValue;
                }

                await Navigation.PopAsync();
            }
            catch
            {

            }
        });
    }
}
