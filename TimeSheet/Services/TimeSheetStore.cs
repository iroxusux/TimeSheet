using PCLStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeSheet.Models;

namespace TimeSheet.Services
{    
    public class TimeSheetStore : IDataStore<UserTimeSheet>
    {
        readonly List<UserTimeSheet> TimeSheets;
        public TimeSheetStore()
        {
            TimeSheets = new List<UserTimeSheet>();
        }
        public async Task<bool> AddItemAsync(UserTimeSheet oTimeSheet)
        {
            TimeSheets.Add(oTimeSheet);
            await SaveItemLocal(oTimeSheet);
            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(UserTimeSheet oTimeSheet)
        {
            var oOldItem = TimeSheets.Where((UserTimeSheet oArg) => oArg.Id == oTimeSheet.Id).FirstOrDefault();
            TimeSheets.Remove(oOldItem);
            TimeSheets.Add(oTimeSheet);
            await SaveItemLocal(oTimeSheet);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(string sId)
        {
            var oOldItem = TimeSheets.Where((UserTimeSheet oArg) => oArg.Id == sId).FirstOrDefault();
            await DeleteItemLocal(oOldItem);
            TimeSheets.Remove(oOldItem);
            return await Task.FromResult(true);
        }
        public async Task<UserTimeSheet> GetItemAsync(string sId)
        {
            return await Task.FromResult(TimeSheets.FirstOrDefault(s => s.Id == sId));
        }
        public async Task<IEnumerable<UserTimeSheet>> GetItemsAsync(bool bForceRefresh = false)
        {
            return await Task.FromResult(TimeSheets);
        }
        public async Task<bool> SaveItemLocal(UserTimeSheet oTimeSheet)
        {
            string sFileName = oTimeSheet.Id + ".ints";
            IFile iFile = await PCLHelper.CreateFile(sFileName);
            XmlSerialization.WriteToXmlFile(iFile.Path, oTimeSheet);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemLocal(UserTimeSheet oTimeSheet)
        {
            string sFileName = oTimeSheet.Id + ".ints";
            await PCLHelper.DeleteFile(sFileName);
            return await Task.FromResult(true);
        }
    }
}
