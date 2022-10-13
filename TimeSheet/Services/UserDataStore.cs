using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Models;

namespace TimeSheet.Services
{
    public class UserDataStore : IDataStore<User>
    {
        readonly List<User> Users;
        private const string USER_FILE_NAME = "UserInfo.mgt";
        public UserDataStore()
        {
            Users = new List<User>();
        }
        public async Task<bool> AddItemAsync(User item)
        {
            Users.Add(item);
            await SaveItemLocal(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetItemAsync(string id)
        {
            return await Task.FromResult(Users.FirstOrDefault(s => s.AccessibleName == id));
        }

        public async Task<IEnumerable<User>> GetItemsAsync(bool forceRefresh = false)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(User item)
        {
            var oOldItem = Users.Where((User oArg) => oArg.AccessibleName == item.AccessibleName).FirstOrDefault();
            Users.Remove(oOldItem);
            Users.Add(item);
            await SaveItemLocal(item);
            return await Task.FromResult(true);
        }
        public async Task<bool> SaveItemLocal(User item)
        {
            IFile iFile = await PCLHelper.CreateFile(USER_FILE_NAME);
            XmlSerialization.WriteToXmlFile(iFile.Path, item);
            return await Task.FromResult(true);
        }
    }
}
