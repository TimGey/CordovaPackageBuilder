using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public interface ILoggerService
    {
        void SetPathLog(string path);
        void AddLog(int typeLog, string msg);
        void ClosingLogger();
    }
}
