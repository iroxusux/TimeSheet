using PCLStorage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TimeSheet.Services
{
    public static class PCLHelper
    {
        public async static Task<bool> FileExists(this string sFileName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult eFolderExists = await iFolder.CheckExistsAsync(sFileName);
            return (eFolderExists == ExistenceCheckResult.FileExists);
        }
        public async static Task<bool> FolderExists(this string sFolderName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult eFolderExists = await iFolder.CheckExistsAsync(sFolderName);
            return (eFolderExists == ExistenceCheckResult.FileExists);
        }
        public async static Task<IFolder> CreateFolder(this string sFolderName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            iFolder = await iFolder.CreateFolderAsync(sFolderName, CreationCollisionOption.FailIfExists);
            return iFolder;
        }
        public async static Task<IFile> CreateFile(this string sFileName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            IFile iFile = await iFolder.CreateFileAsync(sFileName, CreationCollisionOption.ReplaceExisting);
            return iFile;
        }
        public async static Task<IFile> GetFile(this string sFileName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            bool bExists = await sFileName.FileExists(iFolder);
            if (bExists)
            {
                IFile iFile = await iFolder.GetFileAsync(sFileName);
                return iFile;
            }
            return null;
        }
        public async static Task<bool> DeleteFile(this string sFileName, IFolder iRootFolder = null)
        {
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            bool bExists = await sFileName.FileExists(iFolder);
            if (bExists)
            {
                IFile iFile = await iFolder.GetFileAsync(sFileName);
                await iFile.DeleteAsync();
                return true;
            }
            return false;
        }
        public async static Task<bool> WriteTextAll(this string sFileName, string sContent = "", IFolder iRootFolder = null)
        {
            IFile iFile = await sFileName.CreateFile(iRootFolder);
            await iFile.WriteAllTextAsync(sContent);
            return true;
        }
        public async static Task<string> ReadTextAll(this string sFileName, IFolder iRootFolder = null)
        {
            string sContent = "";
            IFolder iFolder = iRootFolder ?? FileSystem.Current.LocalStorage;
            bool bExsists = await sFileName.FileExists(iFolder);
            if (bExsists)
            {
                IFile iFile = await iFolder.GetFileAsync(sFileName);
                sContent = await iFile.ReadAllTextAsync();
            }
            return sContent;
        }
    }
}
