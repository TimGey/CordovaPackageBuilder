using Newtonsoft.Json.Linq;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CordovaPackagesBuiler.Entyties
{
    public class Config 
    {
        #region Constant
        public string PATH_CONFIG_XML => _path_config_xml;
        private const string _path_config_xml = @"\config.xml";
        public string PATH_CONFIG_CONSTANT_JS => _path_config_constant_js;
        private const string _path_config_constant_js = @"\www\js\config.constant.js";
        public string PATH_NEXWORD_MODULE_JS { get { return _path_nexworld_module_js; } }
        private const string _path_nexworld_module_js = @"\www\js\nexworld\nexworld.module.js";
        public JObject CONFIG_JSON => _config_json;
        private JObject _config_json;
        public string Aapt => _aapt;
        private string _aapt;


        #endregion

        public Config()
        {
            // recuperation du config.json
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().GetName().CodeBase);
            var pathfile = new Uri(Path.Combine(path, "config", @"config.json")).AbsolutePath;
            var pathAapt = new Uri(Path.Combine(path, "Utilitaires")).AbsolutePath;

            try
            {
                _config_json = JObject.Parse(File.ReadAllText(pathfile).ToString());
                _aapt = pathAapt.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().ToString());
            }
            // end config.json

        }

    }

}
