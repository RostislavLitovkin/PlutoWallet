using PlutoWallet.Components.AddressView;
using PlutoWallet.Components.WebView;

namespace PlutoWallet.Components.Nft;

public partial class NftOwnerView : ContentView
{
    public static readonly BindableProperty AddressProperty = BindableProperty.Create(
        nameof(Address), typeof(string), typeof(NftOwnerView),
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanging: (bindable, oldValue, newValue) =>
        {
            var control = (NftOwnerView)bindable;
            var address = (string)newValue;

            control.addressLabel.Text = address.Length switch
            {
                > 12 => address.Substring(0, 12) + "..",
                _ => address,
            };
        });

    public NftOwnerView()
    {
        InitializeComponent();
    }

    public string Address
    {
        get => (string)GetValue(AddressProperty);

        set => SetValue(AddressProperty, value);
    }

    private async void OnTapped(System.Object sender, System.EventArgs e)
    {
        await CopyAddress.CopyToClipboardAsync((string)GetValue(AddressProperty));
    }

    private async void OnSubscanClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new WebViewPage($"https://www.subscan.io/account/{Address}"));
    }
}