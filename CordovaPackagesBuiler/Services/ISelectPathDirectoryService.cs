using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public  interface ISelectPathDirectoryService
    {
        void SelectPath(string[] tPathFile);
        string SelectFolder();
    }
}
