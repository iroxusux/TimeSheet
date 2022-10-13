using System;
using System.Collections.Generic;
using System.Text;

namespace TimeSheet.Services
{
    public interface IAlertMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
