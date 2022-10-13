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

namespace TimeSheet.Droid
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    internal class AlarmHandler : LocalBroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if(intent?.Extras != null)
            {
                string title = intent.GetStringExtra(NotificationManagerAndroid.TitleKey);
                string message = intent.GetStringExtra(NotificationManagerAndroid.MessageKey);

                NotificationManagerAndroid manager = NotificationManagerAndroid.Instance ?? new NotificationManagerAndroid();
                manager.Show(title, message);
            }
        }
    }
}