using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeSheet.Services;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace TimeSheet.Droid
{
    internal class LocalBroadcastReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            PowerManager pm = (PowerManager)context.GetSystemService(Context.PowerService);
            PowerManager.WakeLock wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "BackgroundReceiver");
            wakeLock.Acquire();

            if (intent.Action.Equals(Intent.ActionBootCompleted))
            {
                Toast.MakeText(context, "boot completed", ToastLength.Short).Show();
            }

            // Start Services As Required
            Intent myIntent = new Intent(Android.App.Application.Context, typeof(PeriodicService));
            PendingIntent pendingIntent = PendingIntent.GetService(Android.App.Application.Context, 0, myIntent, PendingIntentFlags.Immutable);
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            alarmManager.SetRepeating(AlarmType.RtcWakeup, 0, 5000, pendingIntent);

            wakeLock.Release();
        }
    }
}