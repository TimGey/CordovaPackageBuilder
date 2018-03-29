using CordovaPackagesBuiler.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
   public interface IModeDeploimentService
    {
        ModeDeploiment CreateModeDeploid(string deploiment, string VersionName, string VersionCode);
        ModeDeploiment AddPlatform(ModeDeploiment mdd, string platform);
    }
}
