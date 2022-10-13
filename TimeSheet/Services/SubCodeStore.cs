using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Models;

namespace TimeSheet.Services
{
    internal class SubCodeStore : IDataStore<SubCode>
    {
        readonly List<SubCode> subCodes;
        public SubCodeStore()
        {
            subCodes = new List<SubCode>();
        }
        public async Task<bool> AddItemAsync(SubCode item)
        {
            subCodes.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = subCodes.Where((SubCode subCode) => subCode.Id == id).FirstOrDefault();
            subCodes.Remove(oldItem);
            return await Task.FromResult(true);
        }

        public async Task<SubCode> GetItemAsync(string id)
        {
            return await Task.FromResult(subCodes.Where((SubCode subCode) => subCode.Id == id).FirstOrDefault());
        }

        public async Task<IEnumerable<SubCode>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(subCodes);
        }

        public async Task<bool> UpdateItemAsync(SubCode item)
        {
            var oldItem = subCodes.Where((SubCode subCode) => subCode.Id == item.Id).FirstOrDefault();
            subCodes.Remove(oldItem);
            subCodes.Add(item);
            return await Task.FromResult(true);
        }
    }
}
