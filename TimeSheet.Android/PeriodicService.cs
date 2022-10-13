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
using TimeSheet.Models;
using TimeSheet.Services;
using Xamarin.Forms;

namespace TimeSheet.Droid
{
    [Service(Enabled = true, Exported = true)]
    public class PeriodicService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            IDataStore<UserTimeSheet> TimeSheetDataStore = DependencyService.Get<IDataStore<UserTimeSheet>>();
            List<UserTimeSheet> userTimeSheets = (List<UserTimeSheet>)TimeSheetDataStore.GetItemsAsync().Result;

            // Notify user that they need to submit their time sheet on Sunday.
            if(DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                foreach (UserTimeSheet timeSheet in userTimeSheets)
                {
                    if (!timeSheet.Submitted)
                    {
                        string title = "Time Sheet Submission Required";
                        string message = $"Time Sheet (Week Ending {timeSheet.WeekEndingDateString}) Needs To Be Submitted.";
                        DependencyService.Get<INotificationManager>().SendNotification(title, message);
                        return StartCommandResult.Sticky;
                    }
                }
            }
            // Notify user that they need to fill out their time for the day
            TimeSpan start = new TimeSpan(18, 0, 0);
            TimeSpan end = new TimeSpan(20, 0, 0);
            TimeSpan now = DateTime.Now.TimeOfDay;
            // Don't bother people on saturday and sunday (unless it's about submitting your time sheet)
            if(now > start && now < end && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                string title = "Time Sheet Reminder";
                string message = $"Remember to fill out your time for today!";
                DependencyService.Get<INotificationManager>().SendNotification(title, message);
                return StartCommandResult.Sticky;
            }
            return StartCommandResult.Sticky;
        }
    }
}