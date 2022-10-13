using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TimeSheet.Models
{
    public class TimeSheetTaskDailyHours : INotifyPropertyChanged
    {
        private double _StandardHours = 0.0;
        public double StandardHours
        {
            get { return _StandardHours; }
            set
            {
                _StandardHours = value;
                OnStandardHoursChanged(EventArgs.Empty);
            }
        }
        private double _OvertimeHours = 0.0;
        public double OvertimeHours
        {
            get { return _OvertimeHours; }
            set
            {
                _OvertimeHours = value;
                OnOvertimeHoursChanged(EventArgs.Empty);
            }
        }

        #region HoursChanged
        public event EventHandler StandardHoursChanged = delegate { };
        public event EventHandler OvertimeHoursChanged = delegate { };
        private void OnStandardHoursChanged(EventArgs oArgs)
        {
            EventHandler oHandler = StandardHoursChanged;
            oHandler?.Invoke(this, oArgs);
            OnPropertyChanged("StandardHours");
        }
        private void OnOvertimeHoursChanged(EventArgs oArgs)
        {
            EventHandler oHandler = OvertimeHoursChanged;
            oHandler?.Invoke(this, oArgs);
            OnPropertyChanged("OvertimeHours");
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
