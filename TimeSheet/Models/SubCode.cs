using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TimeSheet.Services;

namespace TimeSheet.Models
{
    public class SubCode
    {
        public string Id { get; set; }
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _Number;
        public string Suffix
        {
            get { return _Number; }
            set
            {
                _Number = value;
                OnPropertyChanged(nameof(Suffix));
            }
        }

        public string DisplayString => $"{Suffix} - {Name}";

        public SubCode()
        {

        }
        public async Task<bool> Init()
        {
            Id = Guid.NewGuid().ToString();
            return await Task.FromResult(true);
        }
        public static string ParseFromDisplayString(string displayString)
        {
            string[] strings = displayString.Split(' ');
            return strings[0];
        }
        public static string RestoreToDisplayString(string subCode, IEnumerable<SubCode> subCodeList)
        {
            return subCodeList.Where(x => x.DisplayString.Contains(subCode)).FirstOrDefault().DisplayString;
        }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
