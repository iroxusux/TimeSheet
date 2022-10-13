using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Content;
using Xamarin.Forms;
using TimeSheet.Services;
using Android.Text.Format;
using Xamarin.Forms.Platform.Android;

namespace TimeSheet.Droid
{
    [Activity(Label = "TimeSheet", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            RegisterAlarmManager();
            

            App.ParentWindow = this;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }
        private void CreateNotificationFromIntent(Intent intent)
        {
            if(intent?.Extras != null)
            {
                string title = intent.GetStringExtra(NotificationManagerAndroid.TitleKey);
                string message = intent.GetStringExtra(NotificationManagerAndroid.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }
        public void RegisterAlarmManager()
        {
            // Start Services As Required
            Intent intent = new Intent(this, typeof(PeriodicService));
            intent.AddFlags(ActivityFlags.ReceiverForeground);
            intent.AddFlags(ActivityFlags.IncludeStoppedPackages);
            PendingIntent pendingIntent = PendingIntent.GetService(this, 0, intent, PendingIntentFlags.Immutable);
            AlarmManager alarmManager = (AlarmManager)GetSystemService(Context.AlarmService);
            alarmManager.SetRepeating(AlarmType.RtcWakeup, 0, 10_000, pendingIntent);
        }
    }
}