﻿using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using TimeSheet.Droid;
using TimeSheet.Models;
using TimeSheet.Services;
using DependencyAttribute = Xamarin.Forms.DependencyAttribute;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(NotificationManagerAndroid))]
namespace TimeSheet.Droid
{
    internal class NotificationManagerAndroid : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static NotificationManagerAndroid Instance { get; private set; }

        public NotificationManagerAndroid() => Initialize();

        public void Initialize()
        {
            if(Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message);
            }
        }

        public void Show(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            PendingIntent pendingIntent = null;
            try
            {
                pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            }
            catch(Exception e)
            {
                if(e.InnerException != null)
                {
                    string err = e.InnerException.Message;
                }
                return;
            }
            

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamarin_logo))
                .SetSmallIcon(Resource.Drawable.xamarin_logo)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime; // milliseconds
        }
    }
}