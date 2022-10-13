using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    public class UserTimeSheet : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public TimeSheetTask[] Tasks { get; set; } = new TimeSheetTask[7];
        public double AllStandardHours
        {
            get
            {
                double x = 0;
                for(int i = 0; i < Tasks.Length; i++)
                {
                    x += Tasks[i].AllStandardHours;
                }
                return x;
            }
        }
        public double AllOvertimeHours
        {
            get
            {
                double x = 0;
                for(int i = 0; i < Tasks.Length; i++)
                {
                    x += Tasks[i].AllOvertimeHours;
                }
                return x;
            }
        }
        private DateTime _WeekEndingDate;
        public DateTime WeekEndingDate
        {
            get { return _WeekEndingDate; }
            set
            {
                _WeekEndingDate = value;
                OnPropertyChanged("WeekEndingDate");
            }
        }
        public string WeekEndingDateString
        {
            get
            {
                return $"{WeekEndingDate.Month}/{WeekEndingDate.Day}/{WeekEndingDate.Year}";
            }
        }
        private bool _Submitted;
        public bool Submitted
        {
            get { return _Submitted; }
            set
            {
                _Submitted = value;
                OnPropertyChanged(nameof(Submitted));
            }
        }

        public UserTimeSheet()
        {
            
        }
        public async Task<bool> Init()
        {
            Id = Guid.NewGuid().ToString();
            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks[i] = new TimeSheetTask
                {
                    TaskName = $"Task{i + 1}",
                    ParentId = this.Id,
                };
                Tasks[i].StandardHoursChanged += new EventHandler(OnStandardHoursChanged);
                Tasks[i].OvertimeHoursChanged += new EventHandler(OnOvertimeHoursChanged);
                await Tasks[i].Init();
            }
            return await Task.FromResult(true);
        }
        #region HoursByDay
        public double StandardHoursByDay(int day)
        {
            double standardTime = 0;
            foreach(TimeSheetTask task in Tasks)
            {
                standardTime += task.Days[day].StandardHours;
            }
            return standardTime;
        }
        public double OvertimeHoursByDay(int day)
        {
            double overtimeTime = 0;
            foreach(TimeSheetTask task in Tasks)
            {
                overtimeTime += task.Days[day].OvertimeHours;
            }
            return overtimeTime;
        }
        #endregion
        #region HoursChanged
        public event EventHandler StandardHoursChanged = delegate { };
        public event EventHandler OvertimeHoursChanged = delegate { };
        private void OnStandardHoursChanged(object oSender, EventArgs oArgs)
        {
            EventHandler oHandler = StandardHoursChanged;
            oHandler?.Invoke(this, oArgs);
            OnPropertyChanged("AllStandardHours");
        }
        private void OnOvertimeHoursChanged(object oSender, EventArgs oArgs)
        {
            EventHandler oHandler = OvertimeHoursChanged;
            oHandler?.Invoke(this, oArgs);
            OnPropertyChanged("AllOvertimeHours");
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
