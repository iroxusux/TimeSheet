using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Text;

namespace TimeSheet.Services
{
    public interface IResourceContainer
    {
        string Get(string key);
    }
    internal class ResourceContainer : IResourceContainer
    {
        public static string ResourceId = "TimeSheet.Resources";
        private ResourceManager _ResourceManager;

        public ResourceContainer(ResourceManager oManager)
        {
            _ResourceManager = oManager;
        }

        public string Get(string key)
        {
            return _ResourceManager.GetString(key);
        }
    }
}
