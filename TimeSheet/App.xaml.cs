using System;
using TimeSheet.Services;
using TimeSheet.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Graph;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PCLStorage;
using TimeSheet.Models;
using System.Resources;
using System.Reflection;
using User = TimeSheet.Models.User;

namespace TimeSheet
{
    public partial class App : Xamarin.Forms.Application
    {
        public static event EventHandler UserDataLoaded = delegate { };
        private void OnUserDataLoaded(EventArgs oArgs)
        {
            EventHandler oHandler = UserDataLoaded;
            oHandler?.Invoke(this, oArgs);
        }

        public static ResourceManager ResManager = new ResourceManager(ResourceContainer.ResourceId, typeof(App).GetTypeInfo().Assembly);

        public static IPublicClientApplication IdentityClientApp = null;
        public static string ClientID = "91b3a633-49a3-4527-a7ac-0ecd0aca6992";  // Client ID only for this program
        public static string TenantID = "09ebfde1-6505-4c31-942f-18875ff0189d";
        public static string[] Scopes = { "User.Read", "User.ReadBasic.All", "Files.Read", "Files.Read.All", "Files.ReadWrite", "Files.ReadWrite.All" };
        public static AuthenticationResult AuthResult;
        public static object ParentWindow { get; set; }

        public App(string sSpecialRedirectUri = null)
        {
            InitializeComponent();
            // Register local stores
            DependencyService.Register<TimeSheetStore>();
            DependencyService.Register<TimeSheetTaskStore>();
            DependencyService.Register<UserDataStore>();
            DependencyService.Register<SubCodeStore>();
            // Register notifiers
            DependencyService.Register<IAlertMessage>();
            _ = Load();  // Load data stores for proper information
            //IdentityClientApp = PublicClientApplicationBuilder.Create(ClientID).WithRedirectUri(sSpecialRedirectUri ?? "https://login.microsoftonline.com/09ebfde1-6505-4c31-942f-18875ff0189d").Build();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task<bool> Load()
        {
            await LocalLoadTimeSheets();
            await LocalLoadUser();
            await LocalLoadSubCodes();
            return await Task.FromResult(true);
        }
        private async Task<bool> LocalLoadUser(IFolder iRootFolder = null)
        {
            IDataStore<User> UserDataStore = DependencyService.Get<IDataStore<User>>();
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            string[] sFilePaths = System.IO.Directory.GetFiles(iFolder.Path.ToString());
            foreach(string sPath in sFilePaths)
            {
                if (!sPath.EndsWith(".mgt")) continue;
                IFile iFile = await PCLHelper.GetFile(sPath);
                User user = XmlSerialization.ReadFromXmlFile<User>(iFile.Path);
                if(user == null)
                {
                    return await Task.FromResult(false);
                }
                await UserDataStore.AddItemAsync(user);
                return await Task.FromResult(true);
            }
            // No user? let's create a default user for now!
            User newUser = new User();
            newUser.FName = "John";
            newUser.LName = "Doe";
            newUser.CellNumber = "123-456-7890";
            newUser.Email = "JDoe@Indicon.com";
            newUser.EmployeeNumber = "123";
            UserDataStore.AddItemAsync(newUser);
            return await Task.FromResult(false);
        }
        private async Task<bool> LocalLoadTimeSheets(IFolder iRootFolder = null)
        {
            IDataStore<UserTimeSheet> TimeSheetDataStore = DependencyService.Get<IDataStore<UserTimeSheet>>();
            IDataStore<TimeSheetTask> TimeSheetTaskDataStore = DependencyService.Get<IDataStore<TimeSheetTask>>();
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            string[] sFilePaths = System.IO.Directory.GetFiles(iFolder.Path.ToString());
            foreach (string sPath in sFilePaths)
            {
                if (!sPath.EndsWith(".ints")) continue;
                IFile iFile = await PCLHelper.GetFile(sPath);
                UserTimeSheet oTimeSheet = XmlSerialization.ReadFromXmlFile<UserTimeSheet>(iFile.Path);
                if (oTimeSheet == null)
                {
                    await PCLHelper.DeleteFile(sPath); // This file is not decompressable. May as well get rid of it
                    continue;
                }
                foreach (TimeSheetTask oTask in oTimeSheet.Tasks)
                {
                    await TimeSheetTaskDataStore.AddItemAsync(oTask);
                }
                await TimeSheetDataStore.AddItemAsync(oTimeSheet);
            }
            OnUserDataLoaded(EventArgs.Empty);
            return await Task.FromResult(true);
        }
        private async Task<bool> LocalLoadSubCodes()
        {
            IDataStore<SubCode> subCodeStore = DependencyService.Get<IDataStore<SubCode>>();
            // Generate CONSTANT SubCodes
            SubCode managemet = new SubCode()
            {
                Name = "Management",
                Suffix = "1010",
            };
            await managemet.Init();
            await subCodeStore.AddItemAsync(managemet);
            SubCode hardware = new SubCode()
            {
                Name = "Hardware",
                Suffix = "1110",
            };
            await hardware.Init();
            await subCodeStore.AddItemAsync(hardware);
            SubCode software = new SubCode()
            {
                Name = "Software",
                Suffix = "1210",
            };
            await software.Init();
            await subCodeStore.AddItemAsync(software);
            SubCode startUp = new SubCode()
            {
                Name = "Startup",
                Suffix = "1310",
            };
            await startUp.Init();
            await subCodeStore.AddItemAsync(startUp);
            SubCode standBy = new SubCode()
            {
                Name = "Standby",
                Suffix = "1410",
            };
            await standBy.Init();
            await subCodeStore.AddItemAsync(standBy);
            SubCode cadDesign = new SubCode()
            {
                Name = "CAD Design",
                Suffix = "1510",
            };
            await cadDesign.Init();
            await subCodeStore.AddItemAsync(cadDesign);
            SubCode cadAsBuilt = new SubCode()
            {
                Name = "CAD As-Built",
                Suffix = "1610",
            };
            await cadAsBuilt.Init();
            await subCodeStore.AddItemAsync(cadAsBuilt);
            SubCode docs = new SubCode()
            {
                Name = "Docs And Manuals",
                Suffix = "1710",
            };
            await docs.Init();
            await subCodeStore.AddItemAsync(docs);
            SubCode travel = new SubCode()
            {
                Name = "Travel",
                Suffix = "1810",
            };
            await travel.Init();
            await subCodeStore.AddItemAsync(travel);
            SubCode shop = new SubCode()
            {
                Name = "Shop",
                Suffix = "1910",
            };
            await shop.Init();
            await subCodeStore.AddItemAsync(shop);
            SubCode overHead = new SubCode()
            {
                Name = "Overhead",
                Suffix = "6000",
            };
            await overHead.Init();
            await subCodeStore.AddItemAsync(overHead);
            SubCode vacation = new SubCode()
            {
                Name = "Vacation",
                Suffix = "Vac",
            };
            await vacation.Init();
            await subCodeStore.AddItemAsync(vacation);
            SubCode holiday = new SubCode()
            {
                Name = "Holiday",
                Suffix = "Hol",
            };
            await holiday.Init();
            await subCodeStore.AddItemAsync(holiday);

            return await Task.FromResult(true);
        }
        
    }
}
