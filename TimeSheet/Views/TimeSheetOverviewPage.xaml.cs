using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.ViewModels;
using TimeSheet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeSheet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetOverviewPage : ContentPage
    {
        TimeSheetOverviewViewModel _ViewModel;
        public TimeSheetOverviewPage()
        {
            InitializeComponent();
            BindingContext = _ViewModel = new TimeSheetOverviewViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ViewModel.OnAppearing();
        }
    }
}