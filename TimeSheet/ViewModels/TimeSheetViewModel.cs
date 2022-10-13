using Microsoft.Graph;
using PCLStorage;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Models;
using TimeSheet.Services;
using TimeSheet.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;
using File = System.IO.File;
using User = TimeSheet.Models.User;

namespace TimeSheet.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class TimeSheetViewModel : BaseViewModel
    {
        private DateTime _LastDateTime;

        private DateTime _WeekEndingDate;
        public DateTime WeekEndingDate
        {
            get { return _WeekEndingDate; }
            set
            {
                if (value.Date == _LastDateTime.Date) return;
                _WeekEndingDate = value.DayOfWeek == 0 ? value : value.AddDays(7-(int)value.DayOfWeek);
                _LastDateTime = _WeekEndingDate;
                OnPropertyChanged("WeekEndingDate");
            }
        }
        public Command<TimeSheetTask> ItemTapped { get; }
        public Command DeleteTimeSheetCommand { get; }
        public Command SubmitCommand { get; }

        public string Id { get; set; }
        private UserTimeSheet _TimeSheet;
        public UserTimeSheet TimeSheet
        {
            get => _TimeSheet;
            set
            {
                SetProperty(ref _TimeSheet, value);
            }
        }
        public TimeSheetViewModel()
        {
            Title = "Time Sheet";
            ItemTapped = new Command<TimeSheetTask>(OnItemSelected);
            DeleteTimeSheetCommand = new Command(OnDeleteTimeSheet);
            SubmitCommand = new Command(OnSubmitTimeSheet);
        }
        private string _ItemId;
        public string ItemId
        {
            get
            {
                return _ItemId;
            }
            set
            {
                _ItemId = value;
                LoadItemId(value);
            }
        }
        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await TimeSheetDataStore.GetItemAsync(itemId);
                TimeSheet = item;
                WeekEndingDate = TimeSheet.WeekEndingDate;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private async void OnItemSelected(TimeSheetTask oTask)
        {
            if (oTask == null) return;
            if (TimeSheet == null) return;
            await Shell.Current.GoToAsync($"{nameof(TimeSheetTaskPage)}?{nameof(TimeSheetTaskViewModel.ItemId)}={oTask.Id}&TimeSheetId={TimeSheet.Id}");
        }
        private async void OnDeleteTimeSheet(object oObject)
        {
            await TimeSheetDataStore.DeleteItemAsync(TimeSheet.Id);
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnSubmitTimeSheet(object oObject)
        {
            ExcelEngine oEngine = new ExcelEngine();
            IApplication oApplication = oEngine.Excel;
            oApplication.DefaultVersion = ExcelVersion.Xlsx;
            string sResourcePath = "TimeSheet.xx-xx-xx (last_name) Timesheet_v5.0.5.xlsm";
            // App is the class of the portable project
            Assembly oAssembly = typeof(App).GetTypeInfo().Assembly;
            Stream sFileStream = oAssembly.GetManifestResourceStream(sResourcePath);
            if (sFileStream == null)
            {
                DependencyService.Get<IAlertMessage>().ShortAlert("Could not load excel file resource!");
            }

            // Open the workbook
            IWorkbook oWorkBook = oApplication.Workbooks.Open(sFileStream);

            // Get First Worksheet
            IWorksheet oWorkSheet = oWorkBook.Worksheets[0];

            // After workbook and worksheet are properly loaded, load data from user repo
            User user = await UserStore.GetItemAsync("User");

            // Set all values to the worksheet as required from our UserTimeSheet
            oWorkSheet.Range["B3"].Text = user.LName;
            oWorkSheet.Range["H3"].Text = user.FName;
            oWorkSheet.Range["K3"].Text = user.EmployeeNumber;
            oWorkSheet.Range["N3"].Text = TimeSheet.WeekEndingDateString;
            oWorkSheet.Range["H44"].Text = $"{user.FName} {user.LName}";

            // Now fill out tasks
            for (int i = 0; i < TimeSheet.Tasks.Length; i++)
            {
                int asciiOffset = 72; // This + "ii" gives us "H".
                oWorkSheet.Range[$"B{i * 2 + 8}"].Text = TimeSheet.Tasks[i].JobNumber;
                oWorkSheet.Range[$"E{i * 2 + 8}"].Text = TimeSheet.Tasks[i].Subcode;
                oWorkSheet.Range[$"F{i * 2 + 8}"].Text = TimeSheet.Tasks[i].IsNightShift ? "x" : "";
                oWorkSheet.Range[$"G{i * 2 + 8}"].Text = TimeSheet.Tasks[i].IsOutOfTown ? "x" : "";
                oWorkSheet.Range[$"O{i * 2 + 8}"].Text = TimeSheet.Tasks[i].AllStandardHours.ToString();
                oWorkSheet.Range[$"O{(i * 2 + 8) + 1}"].Text = TimeSheet.Tasks[i].AllOvertimeHours.ToString();

                // assert all standard and overtime hours by day
                // Stop judging me. This is easy and took like. a minute. Shut up.
                oWorkSheet.Range[$"H22"].Text = TimeSheet.StandardHoursByDay(0).ToString();
                oWorkSheet.Range[$"H23"].Text = TimeSheet.OvertimeHoursByDay(0).ToString();

                oWorkSheet.Range[$"I22"].Text = TimeSheet.StandardHoursByDay(1).ToString();
                oWorkSheet.Range[$"I23"].Text = TimeSheet.OvertimeHoursByDay(1).ToString();

                oWorkSheet.Range[$"J22"].Text = TimeSheet.StandardHoursByDay(2).ToString();
                oWorkSheet.Range[$"J23"].Text = TimeSheet.OvertimeHoursByDay(2).ToString();

                oWorkSheet.Range[$"K22"].Text = TimeSheet.StandardHoursByDay(3).ToString();
                oWorkSheet.Range[$"K23"].Text = TimeSheet.OvertimeHoursByDay(3).ToString();

                oWorkSheet.Range[$"L22"].Text = TimeSheet.StandardHoursByDay(4).ToString();
                oWorkSheet.Range[$"L23"].Text = TimeSheet.OvertimeHoursByDay(4).ToString();

                oWorkSheet.Range[$"M22"].Text = TimeSheet.StandardHoursByDay(5).ToString();
                oWorkSheet.Range[$"M23"].Text = TimeSheet.OvertimeHoursByDay(5).ToString();

                oWorkSheet.Range[$"N22"].Text = TimeSheet.StandardHoursByDay(6).ToString();
                oWorkSheet.Range[$"N23"].Text = TimeSheet.OvertimeHoursByDay(6).ToString();

                // fill total values as well
                oWorkSheet.Range[$"O22"].Text = TimeSheet.AllStandardHours.ToString();
                oWorkSheet.Range[$"O23"].Text = TimeSheet.AllOvertimeHours.ToString();

                for (int ii = 0; ii < TimeSheet.Tasks[i].Days.Length; ii++)
                {
                    char column = (char)(ii + asciiOffset);
                    oWorkSheet.Range[$"{column}{i * 2 + 8}"].Text = TimeSheet.Tasks[i].Days[ii].StandardHours != 0 ? TimeSheet.Tasks[i].Days[ii].StandardHours.ToString() : String.Empty;
                    oWorkSheet.Range[$"{column}{(i * 2 + 8) + 1}"].Text = TimeSheet.Tasks[i].Days[ii].OvertimeHours != 0 ? TimeSheet.Tasks[i].Days[ii].OvertimeHours.ToString() : String.Empty;
                }
            }
            MemoryStream stream = new MemoryStream();
            oWorkBook.SaveAs(stream);
            oWorkBook.Close();
            oEngine.Dispose();

            // Save the stream into a file
            IFile iFile = await PCLHelper.CreateFile("AutoGeneratedTimeSheet.xlsm");
            FileStream fileStream = File.Open(iFile.Path, FileMode.Create);
            stream.Position = 0;
            stream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();

            try
            {
                IFile file = await PCLHelper.GetFile("AutoGeneratedTimeSheet.xlsm");
                if (file == null)
                {
                    DependencyService.Get<IAlertMessage>().ShortAlert("Error reading file for emailing.");
                    return;
                }
                var message = new EmailMessage
                {
                    Subject = "User Generated Time Sheet Submission",
                    Body = " ",
                    To = new List<string>() { user.Email },
                };
                message.Attachments.Add(new EmailAttachment(file.Path));
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                DependencyService.Get<IAlertMessage>().ShortAlert("Not possible to email from this device.");
                Debug.WriteLine(ex.Message);
                return;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IAlertMessage>().ShortAlert("Email error occured. Please notify developer.");
                Debug.WriteLine(ex.Message);
                return;
            }
            DependencyService.Get<IAlertMessage>().ShortAlert("Action Complete.");

        }
        public async void OnDateSelected(DateTime oDateTime)
        {
            if(TimeSheet == null || oDateTime == null) return;
            TimeSheet.WeekEndingDate = oDateTime.AddDays(7-(int)oDateTime.DayOfWeek);
            await TimeSheetDataStore.UpdateItemAsync(TimeSheet);
        }
    }
}
