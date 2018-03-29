using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
   public interface IConsoleService
    {
        void clearConsole();
        void ConsoleAddText(string message, int cdmsg);

    }
}
