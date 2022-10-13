using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TimeSheet.Models;
using TimeSheet.Services;
using Xamarin.Forms;

namespace TimeSheet.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<User> UserStore => DependencyService.Get<IDataStore<User>>();
        public IDataStore<UserTimeSheet> TimeSheetDataStore => DependencyService.Get<IDataStore<UserTimeSheet>>();
        public IDataStore<TimeSheetTask> TimeSheetTaskDataStore => DependencyService.Get<IDataStore<TimeSheetTask>>();
        public IDataStore<SubCode> SubCodeDataStore => DependencyService.Get<IDataStore<SubCode>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

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
