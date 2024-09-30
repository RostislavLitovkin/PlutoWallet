using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Plugin.Fingerprint;
using Plutonication;
using PlutoWallet.Model;
using PlutoWallet.Platforms.Android;

namespace PlutoWallet;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Intent.ActionView },
    DataScheme = "plutonication",
    AutoVerify = true,
    Categories = new[] {
        Intent.CategoryDefault,
        Intent.CategoryBrowsable
})]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        CrossFingerprint.SetCurrentActivityResolver(() => this);

        if (Intent.Data != null)
        {
            var uriString = Intent?.Data.ToString();

            if (uriString.Equals("plutonication:") || uriString.Equals("plutonication://"))
            {
                // Nothing
            }
            else if (uriString.StartsWith("plutonication"))
            {
                AccessCredentials ac = new AccessCredentials(new Uri(uriString));

                PlutonicationModel.ProcessAccessCredentials(ac);
            }
        }
    }
}
