using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeSheet.Views
{
    public partial class TimeSheetPage : ContentPage
    {
        public TimeSheetPage()
        {
            InitializeComponent();
            BindingContext = new TimeSheetViewModel();
        }
        public void OnDateSelected(object oSender, DateChangedEventArgs oArgs)
        {

        }
    }
}