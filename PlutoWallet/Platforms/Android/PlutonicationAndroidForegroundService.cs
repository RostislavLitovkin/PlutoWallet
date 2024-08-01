using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using PlutoWallet.Model;

namespace PlutoWallet.Platforms.Android
{
    /// <summary>
    /// Sources:
    /// https://learn.microsoft.com/en-us/previous-versions/xamarin/android/app-fundamentals/services/foreground-services,
    /// https://www.youtube.com/watch?v=-eyKlpJI02o,
    /// https://www.youtube.com/watch?v=Q_renpfnbk4,
    /// </summary>
    [Service(ForegroundServiceType = ForegroundService.TypeRemoteMessaging)]
    class PlutonicationAndroidForegroundService : Service
    {
        CancellationTokenSource _cts;
        public const int SERVICE_RUNNING_NOTIFICATION_ID = 96063; // Random id
        
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Notification notification = new AndroidNotificationHelper().GetNotification("Connected via Plutonication");

            StartForeground(SERVICE_RUNNING_NOTIFICATION_ID, notification, ForegroundService.TypeRemoteMessaging);

            _ = Task.Run(() =>
            {
                Task accept = PlutonicationModel.AcceptConnectionAsync();
            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();
                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}
