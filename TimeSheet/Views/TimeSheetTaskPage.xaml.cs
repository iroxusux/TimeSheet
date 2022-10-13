using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeSheet.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimeSheetTaskPage : ContentPage
    {
        public TimeSheetTaskPage()
        {
            InitializeComponent();
        }

        private void OnHoursEntrySelected(object sender, EventArgs e)
        {
            Entry entry = (Entry)sender;
            if (entry == null) return;
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                entry.CursorPosition = 0;
                entry.SelectionLength = entry.Text != null ? entry.Text.Length : 0;
            });
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {

        }
    }
}