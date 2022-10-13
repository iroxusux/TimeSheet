 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Models;

namespace TimeSheet.Services
{
    public class TimeSheetTaskStore : IDataStore<TimeSheetTask>
    {
        readonly List<TimeSheetTask> Tasks;
        public TimeSheetTaskStore()
        {
            Tasks = new List<TimeSheetTask>();
        }
        public async Task<bool> AddItemAsync(TimeSheetTask oTask)
        {
            Tasks.Add(oTask);
            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(TimeSheetTask oTask)
        {
            var oOldItem = Tasks.Where((TimeSheetTask oArg) => oArg.Id == oTask.Id).FirstOrDefault();
            Tasks.Remove(oOldItem);
            Tasks.Add(oTask);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteItemAsync(string id)
        {
            var oOldItem = Tasks.Where((TimeSheetTask oArg) => oArg.Id == id).FirstOrDefault();
            Tasks.Remove(oOldItem);
            return await Task.FromResult(true);
        }
        public async Task<TimeSheetTask> GetItemAsync(string id)
        {
            return await Task.FromResult(Tasks.FirstOrDefault(s => s.Id == id));
        }
        public async Task<IEnumerable<TimeSheetTask>> GetItemsAsync(bool bForceRefresh = false)
        {
            return await Task.FromResult(Tasks);
        }
    }
}
