using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;
using Newtonsoft.Json.Linq;

namespace CordovaPackagesBuiler.Services
{
   public class ModeDeploimentService : IModeDeploimentService
    {


        private Config _config;
        private readonly IConfigurationService _configurationService;

        public ModeDeploimentService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _config = _configurationService.GetConfig();
        }

        public ModeDeploiment AddPlatform(ModeDeploiment mdd, string platform)
        {
            var JDeploiment = JObject.Parse(_config.CONFIG_JSON.GetValue(mdd.ModeName.ToLower()).ToString());
            var Json = JObject.Parse(JDeploiment.GetValue(platform).ToString());
            Package pk = new Package(platform, Json.GetValue("appli_name").ToString(), Json.GetValue("package_name").ToString(), Json.GetValue("deviceType").ToString(), Json.GetValue("cordova_cmd").ToString(), Json.GetValue("path_appli_generate").ToString());
            mdd.Cpackages.Add(pk);

            return mdd;
        }

        public ModeDeploiment CreateModeDeploid(string deploiment, string VersionName, string VersionCode)
        {
            var Mdplt = new ModeDeploiment(deploiment.ToUpper());
            var JDeploiment = JObject.Parse(_config.CONFIG_JSON.GetValue(deploiment).ToString());
            Mdplt.Url = JDeploiment.GetValue("URL").ToString();
            Mdplt.VersionCode = VersionCode;
            Mdplt.VersionName = VersionName;

            return Mdplt;
        }
    }
}
