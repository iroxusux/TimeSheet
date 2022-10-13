using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TimeSheet.Models
{
    public class TimeSheetTask : INotifyPropertyChanged
    {
        public string Id { get; set; }
        public string TaskName { get; set; } = String.Empty;
        private string _ParentId { get; set; }
        public string ParentId
        {
            get { return _ParentId; }
            set
            {
                _ParentId = value;
                OnPropertyChanged("ParentID");
            }
        }
        private string _JobNumber = string.Empty;
        public string JobNumber
        {
            get { return _JobNumber; }
            set
            {
                _JobNumber = value;
                OnPropertyChanged("JobNumber");
            }
        }
        private string _Subcode = string.Empty;
        public string Subcode
        {
            get { return _Subcode; }
            set
            {
                _Subcode = value;
                OnPropertyChanged("Subcode");
            }
        }
        private bool _IsNightShift = false;
        public bool IsNightShift
        {
            get { return _IsNightShift; }
            set
            {
                _IsNightShift = value;
                OnPropertyChanged("IsNightShift");
            }
        }
        private bool _IsOutOfTown = false;
        public bool IsOutOfTown
        {
            get { return _IsOutOfTown; }
            set
            {
                _IsOutOfTown = value;
                OnPropertyChanged("IsOutOfTown");
            }
        }
        public TimeSheetTaskDailyHours[] Days { get; set; } = new TimeSheetTaskDailyHours[7];
        public double AllStandardHours
        {
            get
            {
                double x = 0;
                for(int i = 0; i < Days.Length; i++)
                {
                    x += Days[i].StandardHours;
                }
                return x;
            }
        }
        public double AllOvertimeHours
        {
            get
            {
                double x = 0;
                for(int i = 0; i < Days.Length; i++)
                {
                    x += Days[i].OvertimeHours;
                }
                return x;
            }
        }
        public TimeSheetTask()
        {
            
        }
        public async Task<bool> Init()
        {
            Id = Guid.NewGuid().ToString();
            for (int i = 0; i < Days.Length; i++)
            {
                Days[i] = new TimeSheetTaskDailyHours();
                Days[i].StandardHoursChanged += new EventHandler(OnStandardHoursChanged);
                Days[i].OvertimeHoursChanged += new EventHandler(OnOvertimeHoursChanged);
            }
            return await Task.FromResult(true);
        }
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
