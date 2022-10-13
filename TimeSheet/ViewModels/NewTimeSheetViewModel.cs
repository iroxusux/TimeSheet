using System;
using TimeSheet.Models;
using Xamarin.Forms;

namespace TimeSheet.ViewModels
{
    public class NewTimeSheetViewModel : BaseViewModel
    {
        private DateTime _LastDateTime;

        private DateTime _WeekEndingDate;
        public DateTime WeekEndingDate
        {
            get { return _WeekEndingDate; }
            set
            {
                if (value.Date == _LastDateTime.Date) return;
                _WeekEndingDate = value.DayOfWeek == 0 ? value : value.AddDays(7 - (int)value.DayOfWeek);
                _LastDateTime = _WeekEndingDate;
                OnPropertyChanged("WeekEndingDate");
            }
        }
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public NewTimeSheetViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            WeekEndingDate = DateTime.Now;
            Title = "New Time Sheet";
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            UserTimeSheet oTimeSheet = new UserTimeSheet()
            {
                WeekEndingDate = WeekEndingDate,
            };
            await oTimeSheet.Init();
            foreach (TimeSheetTask oTask in oTimeSheet.Tasks)  // Store each task separately for the rest of the app to use
            {
                await TimeSheetTaskDataStore.AddItemAsync(oTask);
            }
            await TimeSheetDataStore.AddItemAsync(oTimeSheet);  // Store the time sheet for the rest of the app to use


            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
