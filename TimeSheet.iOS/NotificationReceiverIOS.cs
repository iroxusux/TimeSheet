using Foundation;
using ObjCRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimeSheet.Services;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace TimeSheet.iOS
{
    public class NotificationReceiverIOS : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            ProcessNotification(notification);
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
        private void ProcessNotification(UNNotification notification)
        {
            string title = notification.Request.Content.Title;
            string message = notification.Request.Content.Body;
            DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
        }
    }
}