using System;
using System.Collections.Generic;
using TimeSheet.ViewModels;
using TimeSheet.Views;
using Xamarin.Forms;

namespace TimeSheet
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewTimeSheetPage), typeof(NewTimeSheetPage));
            Routing.RegisterRoute(nameof(TimeSheetPage), typeof(TimeSheetPage));
            Routing.RegisterRoute(nameof(TimeSheetTaskPage), typeof(TimeSheetTaskPage));
        }

    }
}
