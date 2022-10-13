using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Services;
using TimeSheet.Views;
using Xamarin.Forms;

namespace TimeSheet.ViewModels
{
    public class TimeSheetOverviewViewModel : BaseViewModel
    {
        #region Commands
        public Command AddTimeSheetCommand { get; }
        public Command LoadItemsCommand { get; }
        public Command<Models.UserTimeSheet> ItemTapped { get; }
        #endregion

        private INotificationManager notificationManager;

        public ObservableCollection<Models.UserTimeSheet> Items { get; }

        private Models.UserTimeSheet _SelectedItem;
        public Models.UserTimeSheet SelectedItem
        {
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                OnItemSelected(value);
            }
        }

        public TimeSheetOverviewViewModel()
        {
            notificationManager = DependencyService.Get<INotificationManager>();
            Title = "Time Sheets Overview";
            Items = new ObservableCollection<Models.UserTimeSheet>();
            AddTimeSheetCommand = new Command(OnAddTimeSheet);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItems());
            ItemTapped = new Command<Models.UserTimeSheet>(OnItemSelected);
            App.UserDataLoaded += new EventHandler(OnUserDataLoaded);
            _ = ExecuteLoadItems();
        }
        private void OnUserDataLoaded(object oSender, EventArgs oArgs)
        {
            _ = ExecuteLoadItems();
        }
        private async void OnAddTimeSheet(object oObject)
        {
            await Shell.Current.GoToAsync(nameof(NewTimeSheetPage));
        }
        private async Task ExecuteLoadItems()
        {
            IsBusy = true;
            try
            {
                Items.Clear();
                var oNewItems = await TimeSheetDataStore.GetItemsAsync(true);
                foreach(var oItem in oNewItems)
                {
                    Items.Add(oItem);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void OnAppearing()
        {
            IsBusy = true;
            _SelectedItem = null;
        }
        private async void OnItemSelected(Models.UserTimeSheet oTimeSheet)
        {
            if (oTimeSheet == null) return;
            await Shell.Current.GoToAsync($"{nameof(TimeSheetPage)}?{nameof(TimeSheetViewModel.ItemId)}={oTimeSheet.Id}");
        }
    }
}
