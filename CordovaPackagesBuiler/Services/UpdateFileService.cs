using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CordovaPackagesBuiler.Entyties;

namespace CordovaPackagesBuiler.Services
{
  public  class UpdateFileService : IUpdateFileService
    {
        private readonly IBackupFile _backupFile;
        private readonly IConsoleService _consoleService;

        public UpdateFileService(IBackupFile backupFile, IConsoleService consoleService)
        {
            _backupFile = backupFile;
            _consoleService = consoleService;
        }

        public bool CreateConfigConstantJS(ModeDeploiment dmD, string PathDirectory, string PATH_CONFIG_CONSTANT_JS)
        {
            bool result = false;

            try
            {
                _backupFile.MoveFileToBackup(PathDirectory + PATH_CONFIG_CONSTANT_JS, "config.constant.js", PathDirectory);

                var containt = "angular.module(\"mobileGestionVisite\")\n";
                containt += "   .constant(\"config\", {\n";
                containt += "//------------------Generat by CordovaPackagesBuilder----------------------\n";
                containt += "       MAX_PICTURES: 3,\n";
                containt += "       APPLICATION_IDENT: \"FONCIA-GESTION-VISITE\",\n";
                containt += "       APPLICATION_IDENT_PHONE: \"FONCIA-GESTION-VISITE-SMARTPHONE\",\n";
                containt += "       VERSION_NAME:\"" + dmD.VersionName + "\",\n";
                containt += "       VERSION_IDENT:" + dmD.VersionCode + ",\n";
                containt += "       PREPROD:" + (dmD.ModeName == "PREPROD").ToString().ToLower() + ",\n";
                containt += "       MEAP_URL:" + dmD.Url + "\n";
                containt += " });";
                StreamWriter ConfigConstantJs = new StreamWriter(PathDirectory + PATH_CONFIG_CONSTANT_JS, false, System.Text.Encoding.ASCII);
                ConfigConstantJs.Write(containt);
                ConfigConstantJs.Close();

                result = true;
            }
            catch (Exception e)
            {
                _consoleService.ConsoleAddText(e.ToString(), 2);
                result = false;
            }
            return result;
        }

        public bool UpdateConfigXml(ModeDeploiment dmD, string PathDirectory, string PATH_CONFIG_XML)
        {
            bool result;
            _backupFile.MoveFileToBackup(PathDirectory + PATH_CONFIG_XML, "config.xml", PathDirectory);
            try
            {
                var ConfigXml = XDocument.Load(Path.Combine(PathDirectory + PATH_CONFIG_XML));
                var widget = ConfigXml.Elements().SingleOrDefault();
                var version = widget.Attribute("version");
                version.SetValue(dmD.VersionName);
                var elements = widget.Elements();
                if (dmD.Cpackages[0].NamePlatform == "android")
                {
                    var package_platform = widget.Attributes("android-packageName").SingleOrDefault();
                    package_platform.Value = dmD.Cpackages[0].NamePackage;
                }
                else if (dmD.Cpackages[0].NamePlatform == "windows")
                {
                    var packageWindows = widget.Attribute("windows-packageVersion");
                    packageWindows.SetValue(dmD.VersionName);
                    var package_platform = widget.Attributes("id").SingleOrDefault();
                    package_platform.Value = dmD.Cpackages[0].NamePackage;
                }

                var name = widget.Elements().First<XElement>();
                if (name != null)
                {
                    name.SetValue(dmD.Cpackages[0].NameApli);
                }

                ConfigXml.Save(Path.Combine(PathDirectory + PATH_CONFIG_XML));
                result = true;
            }
            catch (Exception e)
            {
                var message = e.ToString();
                _consoleService.ConsoleAddText(message, 2);
                result = false;
            }

            return result;
        }

        public bool UpdateNexworldModuleJs(ModeDeploiment mdd, string PathDirectory, string PATH_NEXWORD_MODULE_JS)
        {
            bool result = false;

            try
            {

                _backupFile.MoveFileToBackup(PathDirectory + PATH_NEXWORD_MODULE_JS, "nexworld.module.js", PathDirectory);

                string text = File.ReadAllText(PathDirectory + PATH_NEXWORD_MODULE_JS);
                if (mdd.Cpackages[0].NamePlatform == "android")
                {
                    text = text.Replace("return TABLET", "return PHONE");
                }
                if (mdd.Cpackages[0].NamePlatform == "windows")
                {
                    text = text.Replace("return PHONE", "return TABLET");
                }
                File.WriteAllText(PathDirectory + PATH_NEXWORD_MODULE_JS, text);

                result = true;
            }
            catch (Exception ex)
            {
                _consoleService.ConsoleAddText(ex.ToString(), 2);
                result = false;
            }

            return result;
        }
    }
}
