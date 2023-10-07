using PlutoWallet.Model;
using PlutoWallet.Components.UniversalScannerView;

namespace PlutoWallet.Components.Identity;

public partial class IdentityAddressView : ContentView
{
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(IdentityAddressView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: async(bindable, oldValue, newValue) => {
            var control = (IdentityAddressView)bindable;

            control.addressEntry.Text = (string)newValue;

            // I had to duplicate these lines because of a weird error on Android.

            var identity = await Model.IdentityModel.GetIdentityForAddress((string)newValue);

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
                this.Address = e.Results[e.Results.Length - 1].Value;

                await Navigation.PopAsync();
            }
            catch
            {

            }
        });
    }
}
