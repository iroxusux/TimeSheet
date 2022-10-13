using System;
using System.Collections.Generic;
using System.Text;
using TimeSheet.Models;
using TimeSheet.Services;
using Xamarin.Forms;

namespace TimeSheet.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public Command SaveCommand { get; }

        private string _FName;
        public string FName
        {
            get { return _FName; }
            set
            {
                _FName = value;
                OnPropertyChanged("FName");
            }
        }

        private string _LName;
        public string LName
        {
            get { return _LName; }
            set
            {
                _LName = value;
                OnPropertyChanged("LName");
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                _Email = value;
                OnPropertyChanged("Email");
            }
        }

        private string _CellNumber;
        public string CellNumber
        {
            get { return _CellNumber; }
            set
            {
                _CellNumber = value;
                OnPropertyChanged("CellNumber");
            }
        }

        private string _EmployeeNumber;
        public string EmployeeNumber
        {
            get { return _EmployeeNumber; }
            set
            {
                _EmployeeNumber = value;
                OnPropertyChanged("EmployeeNumber");
            }
        }

        public UserViewModel()
        {
            Title = "User Information";
            LoadUser();
            SaveCommand = new Command(OnSave);
        }
        private async void LoadUser()
        {
            IDataStore<User> UserStore = DependencyService.Get<IDataStore<User>>();
            User user = await UserStore.GetItemAsync("User");
            FName = user.FName;
            LName = user.LName;
            Email = user.Email;
            CellNumber = user.CellNumber;
            EmployeeNumber = user.EmployeeNumber;
        }
        private async void OnSave()
        {
            User user = new User()
            {
                FName = FName,
                LName = LName,
                Email = Email,
                CellNumber = CellNumber,
                EmployeeNumber = EmployeeNumber,
            };
            IDataStore<User> UserStore = DependencyService.Get<IDataStore<User>>();
            await UserStore.UpdateItemAsync(user);
            DependencyService.Get<IAlertMessage>().ShortAlert("User Data Saved.");
        }
    }
}
