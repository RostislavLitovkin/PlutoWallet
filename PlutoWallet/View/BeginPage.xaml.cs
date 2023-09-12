using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using Schnorrkel.Keys;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using static Substrate.NetApi.Mnemonic;

namespace PlutoWallet.View;

public partial class BeginPage : ContentPage
{
	public BeginPage()
	{
        NavigationPage.SetHasNavigationBar(this, false);
        Shell.SetNavBarIsVisible(this, false);

        InitializeComponent();
    }

    async void BeginClicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new MnemonicsPage());
    }

    async void OnSkipClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Preferences.Set("biometricsEnabled", false);

        var mnemonicsArray = Model.KeysModel.GenerateMnemonicsArray();
        string temp = string.Empty;

        foreach (string mnemonic in mnemonicsArray)
        {
            temp += " " + mnemonic;
        }

        var Mnemonics = temp.Trim();

        try
        {
            // Set biometrics
            for (int i = 0; i < 5; i++)
            {
                var request = new AuthenticationRequestConfiguration("Biometric verification", "..");

                var result = await CrossFingerprint.Current.AuthenticateAsync(request);

                if (result.Authenticated)
                {
                    // Fingerprint set, perhaps do with it something in the future

                    Preferences.Set("biometricsEnabled", true);

                    break;
                }
                else
                {

                }
            }
        }
        catch
        {

        }

        // This is default, could be changed in the future or with a setting
        ExpandMode expandMode = ExpandMode.Ed25519;

        var secret = Mnemonic.GetSecretKeyFromMnemonic(Mnemonics, "", BIP39Wordlist.English);

        var miniSecret = new MiniSecret(secret, expandMode);

        Account account = Account.Build(
            KeyType.Sr25519,
            miniSecret.ExpandToSecret().ToBytes(),
            miniSecret.GetPair().Public.Key);

        Preferences.Set(
            "publicKey",
             account.Value
        );

        Preferences.Set(
            "mnemonics",
             Mnemonics
        );

        Preferences.Set(
            "password",
            ""
        );

        Preferences.Set("privateKeyExpandMode", 1);

        Preferences.Set("usePrivateKey", false);

        await Navigation.PushAsync(new BasePage());
    }

    async void OnSaveClicked(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await Navigation.PushAsync(new MnemonicsPage());
    }
}
