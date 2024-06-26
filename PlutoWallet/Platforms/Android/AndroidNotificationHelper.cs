using Android.App;
using Android.Content;

namespace PlutoWallet.Platforms.Android
{

    /// <summary>
    /// Source: https://www.youtube.com/watch?v=Q_renpfnbk4
    /// </summary>
    internal class AndroidNotificationHelper
    {
        private static string foregroundChannelId = "96062"; // Random id

#if ANDROID26_0_OR_GREATER
#pragma warning disable CA1416 // Validate platform compatibility
        private static readonly Context context = global::Android.App.Application.Context;
#pragma warning restore CA1416 // Validate platform compatibility
#endif

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Runs only on android")]
        public Notification GetNotification(string description)
        {
#if ANDROID26_0_OR_GREATER

            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("PlutoWallet", description);

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.Immutable);

            var notificationBuilder = new Notification.Builder(context, foregroundChannelId)
                .SetContentTitle("PlutoWallet")
                .SetContentText(description)
                .SetSmallIcon(CommunityToolkit.Maui.Core.Resource.Drawable.plutowalleticon)
                .SetContentIntent(pendingIntent)
                .SetOngoing(true)
                .SetChannelId(foregroundChannelId);

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "PlutoWallet", NotificationImportance.High);
                NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            return notificationBuilder.Build();
#else
            return null;
#endif
        }
    }
}
