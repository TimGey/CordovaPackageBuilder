using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public interface ICmdCordovaService
    {
        void GeneratePackage(string platform, string DirectoryPath, string CordovaCmd);
    }
}
