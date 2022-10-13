using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TimeSheet.Models;
using TimeSheet.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeSheet.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    [QueryProperty(nameof(TimeSheetId), nameof(TimeSheetId))]
    public class TimeSheetTaskViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }
        private string _JobNumber;
        public string JobNumber
        {
            get { return _JobNumber; }
            set
            {
                _JobNumber = value;
                OnPropertyChanged("JobNumber");
            }
        }
        private bool _IsNightShift;
        public bool IsNightShift
        {
            get { return _IsNightShift; }
            set
            {
                _IsNightShift = value;
                OnPropertyChanged("IsNightShift");
            }
        }
        private bool _IsOutOfTown;
        public bool IsOutOfTown
        {
            get { return _IsOutOfTown; }
            set
            {
                _IsOutOfTown = value;
                OnPropertyChanged("IsOutOfTown");
            }
        }
        private double _MondayST;
        public double MondayST
        {
            get { return _MondayST; }
            set
            {
                _MondayST = value;
                OnPropertyChanged("MondayST");
            }
        }
        private double _MondayOT;
        public double MondayOT
        {
            get { return _MondayOT; }
            set
            {
                _MondayOT = value;
                OnPropertyChanged("MondayOT");
            }
        }
        private double _TuesdayST;
        public double TuesdayST
        {
            get { return _TuesdayST; }
            set
            {
                _TuesdayST = value;
                OnPropertyChanged("TuesdayST");
            }
        }
        private double _TuesdayOT;
        public double TuesdayOT
        {
            get { return _TuesdayOT; }
            set
            {
                _TuesdayOT = value;
                OnPropertyChanged("TuesdayOT");
            }
        }
        private double _WednesdayST;
        public double WednesdayST
        {
            get { return _WednesdayST; }
            set
            {
                _WednesdayST = value;
                OnPropertyChanged("WednesdayST");
            }
        }
        private double _WednesdayOT;
        public double WednesdayOT
        {
            get { return _WednesdayOT; }
            set
            {
                _WednesdayOT = value;
                OnPropertyChanged("WednesdayOT");
            }
        }
        private double _ThursdayST;
        public double ThursdayST
        {
            get { return _ThursdayST; }
            set
            {
                _ThursdayST = value;
                OnPropertyChanged("ThursdayST");
            }
        }
        private double _ThursdayOT;
        public double ThursdayOT
        {
            get { return _ThursdayOT; }
            set
            {
                _ThursdayOT = value;
                OnPropertyChanged("ThursdayOT");
            }
        }
        private double _FridayST;
        public double FridayST
        {
            get { return _FridayST; }
            set
            {
                _FridayST = value;
                OnPropertyChanged("FridayST");
            }
        }
        private double _FridayOT;
        public double FridayOT
        {
            get { return _FridayOT; }
            set
            {
                _FridayOT = value;
                OnPropertyChanged("FridayOT");
            }
        }
        private double _SaturdayST;
        public double SaturdayST
        {
            get { return _SaturdayST; }
            set
            {
                _SaturdayST = value;
                OnPropertyChanged("SaturdayST");
            }
        }
        private double _SaturdayOT;
        public double SaturdayOT
        {
            get { return _SaturdayOT; }
            set
            {
                _SaturdayOT = value;
                OnPropertyChanged("SaturdayOT");
            }
        }
        private double _SundayST;
        public double SundayST
        {
            get { return _SundayST; }
            set
            {
                _SundayST = value;
                OnPropertyChanged("SundayST");
            }
        }
        private double _SundayOT;
        public double SundayOT
        {
            get { return _SundayOT; }
            set
            {
                _SundayOT = value;
                OnPropertyChanged("SundayOT");
            }
        }
        private string _ItemId;
        public string ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                _ItemId = value;
                LoadItemId(value);
            }
        }
        private TimeSheetTask _Task;
        public TimeSheetTask Task
        {
            get
            {
                return _Task;
            }
            set
            {
                _Task = value;
            }
        }
        private UserTimeSheet _TimeSheet;
        public UserTimeSheet TimeSheet
        {
            get { return _TimeSheet; }
            set
            {
                _TimeSheet = value;
                OnPropertyChanged("TimeSheeet");
            }
        }
        private string _TimeSheetId;
        public string TimeSheetId
        {
            get { return _TimeSheetId; }
            set
            {
                _TimeSheetId = value;
                LoadTimeSheetId(value);
            }
        }

        private List<string> _SubCodeStrings;
        public List<string> SubCodeStrings
        {
            get { return _SubCodeStrings; }
            set
            {
                _SubCodeStrings = value;
                OnPropertyChanged(nameof(SubCodeStrings));
            }
        }

        private string _SelectedSubCode;
        public string SelectedSubCode
        {
            get { return _SelectedSubCode; }
            set
            {
                _SelectedSubCode = value;
                OnPropertyChanged(nameof(SelectedSubCode));
            }
        }

        public TimeSheetTaskViewModel()
        {
            Title = "Time Sheet Task";
            SaveCommand = new Command(OnSave);
            SubCodeStrings = new List<string>();
            LoadSubCodes();
        }
        private async void LoadSubCodes()
        {
            var subCodes = await SubCodeDataStore.GetItemsAsync();
            foreach(SubCode subCode in subCodes)
            {
                SubCodeStrings.Add(subCode.DisplayString);
            }
        }
        public async void LoadItemId(string sItemId)
        {
            try
            {
                var item = await TimeSheetTaskDataStore.GetItemAsync(sItemId);
                Task = item;
                JobNumber = Task.JobNumber;
                SelectedSubCode = Models.SubCode.RestoreToDisplayString(Task.Subcode, await SubCodeDataStore.GetItemsAsync());
                IsNightShift = Task.IsNightShift;
                IsOutOfTown = Task.IsOutOfTown;
                MondayST = Task.Days[0].StandardHours;
                MondayOT = Task.Days[0].OvertimeHours;
                TuesdayST = Task.Days[1].StandardHours;
                TuesdayOT = Task.Days[1].OvertimeHours;
                WednesdayST = Task.Days[2].StandardHours;
                WednesdayOT = Task.Days[2].OvertimeHours;
                ThursdayST = Task.Days[3].StandardHours;
                ThursdayOT = Task.Days[3].OvertimeHours;
                FridayST = Task.Days[4].StandardHours;
                FridayOT = Task.Days[4].OvertimeHours;
                SaturdayST = Task.Days[5].StandardHours;
                SaturdayOT = Task.Days[5].OvertimeHours;
                SundayST = Task.Days[6].StandardHours;
                SundayOT = Task.Days[6].OvertimeHours;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load task item.");
            }
        }
        public async void LoadTimeSheetId(string sItemId)
        {
            try
            {
                var item = await TimeSheetDataStore.GetItemAsync(sItemId);
                TimeSheet = item;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to load time sheet item.");
            }
        }
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSave()
        {
            if (Task == null || TimeSheet == null) await Shell.Current.GoToAsync(".."); // Pop this off the navigation stack. We can't save.
            Task.JobNumber = JobNumber;
            Task.Subcode = SelectedSubCode != null ? Models.SubCode.ParseFromDisplayString(SelectedSubCode) : string.Empty;
            Task.IsNightShift = IsNightShift;
            Task.IsOutOfTown = IsOutOfTown;
            Task.Days[0].StandardHours = MondayST;
            Task.Days[0].OvertimeHours = MondayOT;
            Task.Days[1].StandardHours = TuesdayST;
            Task.Days[1].OvertimeHours = TuesdayOT;
            Task.Days[2].StandardHours = WednesdayST;
            Task.Days[2].OvertimeHours = WednesdayOT;
            Task.Days[3].StandardHours = ThursdayST;
            Task.Days[3].OvertimeHours = ThursdayOT;
            Task.Days[4].StandardHours = FridayST;
            Task.Days[4].OvertimeHours = FridayOT;
            Task.Days[5].StandardHours = SaturdayST;
            Task.Days[5].OvertimeHours = SaturdayOT;
            Task.Days[6].StandardHours = SundayST;
            Task.Days[6].OvertimeHours = SundayOT;
            await TimeSheetTaskDataStore.UpdateItemAsync(Task);  // Store the time sheet task for the rest of the app to use
            await TimeSheetDataStore.UpdateItemAsync(TimeSheet);
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
