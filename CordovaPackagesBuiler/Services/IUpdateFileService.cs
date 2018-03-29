using CordovaPackagesBuiler.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public  interface IUpdateFileService
    {
        bool UpdateConfigXml(ModeDeploiment dmD, string PathDirectory, string PATH_CONFIG_XML);
        bool CreateConfigConstantJS(ModeDeploiment dmD, string PathDirectory, string PATH_CONFIG_CONSTANT_JS);
        bool UpdateNexworldModuleJs(ModeDeploiment mdd, string PathDirectory, string PATH_NEXWORD_MODULE_JS);
    }
}
