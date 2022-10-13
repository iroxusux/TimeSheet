using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.ViewModels;
using TimeSheet.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace TimeSheet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTimeSheetPage : ContentPage
    {
        private DateTime _LastDate = DateTime.Now;
        public Models.UserTimeSheet TimeSheet { get; }

        public NewTimeSheetPage()
        {
            InitializeComponent();
        }

        public void OnDateSelected(object oSender, DateChangedEventArgs oArgs)
        {

        }
    }
}