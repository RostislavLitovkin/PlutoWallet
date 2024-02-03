using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.Fingerprint;
using Plutonication;
using PlutoWallet.Components.ConnectionRequestView;

namespace PlutoWallet;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { Android.Content.Intent.ActionView },
    DataScheme = "plutonication",
    AutoVerify = true,
    Categories = new[] {Android.Content.Intent.ActionView, Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable })]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        CrossFingerprint.SetCurrentActivityResolver(() => this);

        if (Intent.Data != null)
        {
            var uriString = Intent?.Data.ToString();

            if (uriString.StartsWith("plutonication"))
            {
                AccessCredentials ac = new AccessCredentials(new Uri(uriString));

                var connectionRequest = DependencyService.Get<ConnectionRequestViewModel>();

                connectionRequest.IsVisible = true;
                connectionRequest.Icon = ac.Icon;
                connectionRequest.Name = ac.Name;
                connectionRequest.Url = ac.Url;
                connectionRequest.Key = ac.Key;
                connectionRequest.AccessCredentials = ac;
            }
        }
    }
}
