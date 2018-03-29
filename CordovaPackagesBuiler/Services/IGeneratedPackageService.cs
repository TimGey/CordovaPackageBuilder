using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public interface IGeneratedPackageService
    {
        void StartGeneratedPakage(string plateform, string deploiment, string VersionCode, string VersionName, string PathDirectory);
    }
}
