using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;
using Prism.Events;
using CordovaPackagesBuiler.Events;
using System.Threading;
using System.Xml.Linq;

namespace CordovaPackagesBuiler.Services
{
    class GeneratedPackagesService : IGeneratedPackageService
    {
        #region Properties
        private readonly IModeDeploimentService _modeDeploimentService;
        private readonly IBackupFile _backupFile;
        private readonly IConsoleService _consoleService;
        private readonly IUpdateFileService _updateFileService;
        private readonly ICmdCordovaService _cmdCordovaService;
        private readonly IConfigurationService _configurationService;
        private readonly ISelectPathDirectoryService _selectPathDirectoryService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerService _loggerService;
        private Config _config;
        private string _pathdirectory;
        private string _pathPackageDirectory;
        private ModeDeploiment _mdDplt;
        private string _versionCode;

        #endregion

        #region constructeur
        public GeneratedPackagesService(IModeDeploimentService modeDeploimentService,
                                        IBackupFile backupFile,
                                        IConsoleService consoleService,
                                        IUpdateFileService updateFileService,
                                        ICmdCordovaService cmdCordovaService,
                                        IConfigurationService configurationService,
                                        ISelectPathDirectoryService selectPathDirectoryService,
                                        IEventAggregator eventAggregator,
                                        ILoggerService loggerService)
        {
            _modeDeploimentService = modeDeploimentService;
            _backupFile = backupFile;
            _consoleService = consoleService;
            _updateFileService = updateFileService;
            _cmdCordovaService = cmdCordovaService;
            _configurationService = configurationService;
            _selectPathDirectoryService = selectPathDirectoryService;
            _eventAggregator = eventAggregator;
            _loggerService = loggerService;
            _config = _configurationService.GetConfig();
            if (!_eventAggregator.GetEvent<CmdIsFinishEvent>().Contains(OnFinishRecevied))
                _eventAggregator.GetEvent<CmdIsFinishEvent>().Subscribe(OnFinishRecevied, false);
        }
        #endregion

        #region Event

        private void OnFinishRecevied()
        {
            OnCmdFinish();
        }

        private void OnCmdFinish()
        {
            //--restauration des fichiers d'oigine--//
            RestaurationOriginFiles(_pathdirectory, _mdDplt);

            if (_pathPackageDirectory != "Chemin de destination des packages générés")
            {
                MovePackage(_mdDplt, _pathdirectory, _pathPackageDirectory);
            }

        }
        #endregion

        public void StartGeneratedPakage(string plateform, string deploiment, string VersionIdent, string VersionCode, string VersionName, string PathDirectory, string PathPackageDirectory)
        {
            //on block le bouton pour générer le package
            _eventAggregator.GetEvent<IsBuildableEvent>().Publish(true);
            _pathdirectory = PathDirectory;
            _pathPackageDirectory = PathPackageDirectory;
            _loggerService.SetPathLog(PathPackageDirectory);

            //--vérification des fichers si lecture seul--//
            if (!ReadOnlyFile(PathDirectory, new string[] { _config.PATH_CONFIG_XML, _config.PATH_CONFIG_CONSTANT_JS, _config.PATH_NEXWORD_MODULE_JS }))
            {
                //--instanciation d'un ModeDeploiment && ajout d'une platform--//
                ModeDeploiment MdDplt = _modeDeploimentService.CreateModeDeploid(deploiment, VersionName, VersionIdent, VersionCode);
                MdDplt = _modeDeploimentService.AddPlatform(MdDplt, plateform);
                _mdDplt = MdDplt;
                //--création du dossier backup pour les fichiers d'origine--//
                _backupFile.CreateDirectory(PathDirectory, new string[] { @"\OldConfig" });

                if (UpdateFiles(MdDplt, PathDirectory))
                {
                    //--lancement de la cmd pour build le package--//
                    _cmdCordovaService.CMDExecute(PathDirectory, MdDplt.Cpackages[0].CordovaCmd, true);

                }

            }

        }




        private bool ReadOnlyFile(string pathDirectory, string[] tpathFiles)
        {
            var result = false;

            foreach (var file in tpathFiles)
            {
                if (_backupFile.FileIsOpennable(pathDirectory + file))
                {
                    _consoleService.ConsoleAddText("Le ficher " + file + " est en lecture seul", 1);
                    result = true;
                }
            }
            return result;
        }

        #region UpdateFiles
        private bool UpdateFiles(ModeDeploiment mdd, string PathDirectory)
        {
            var result = false;
            bool xml = _updateFileService.UpdateConfigXml(mdd, PathDirectory, _config.PATH_CONFIG_XML);
            bool constant = _updateFileService.CreateConfigConstantJS(mdd, PathDirectory, _config.PATH_CONFIG_CONSTANT_JS);
            bool module = _updateFileService.UpdateNexworldModuleJs(mdd, PathDirectory, _config.PATH_NEXWORD_MODULE_JS);

            if (xml && constant && module)
            {
                result = true;
            }

            return result;
        }
        #endregion

        #region RemovePackageToFinalDirectory
        private void MovePackage(ModeDeploiment mdd, string PathDirectory, string PathPackageDirectory)
        {
            var nameFolderpackage = @"\" + DateTime.Now.Date.ToString("yyyyMMdd") + "_FONCIA_VISITE_" + mdd.ModeName.ToUpper() + "_" + mdd.VersionIdent;
            string[] tFolder = new string[] { @"\LivraisonFoncia" + "_" + DateTime.Now.Date.ToString("yyyyMMdd")+"-"+DateTime.Now.Hour+DateTime.Now.Minute, @"\Version", @"\" + mdd.Cpackages[0].NamePlatform.ToUpper(), nameFolderpackage };

            _backupFile.CreateDirectory(PathPackageDirectory, tFolder);

            string pathPackage = PathPackageDirectory + string.Join("", tFolder);

            if (mdd.Cpackages[0].NamePlatform.ToLower() == "android")
            {
                MoveAndroidPackage(mdd, PathDirectory, pathPackage);
                Thread.Sleep(3000);
                _loggerService.ClosingLogger();
                File.Move(_pathPackageDirectory + @"\log\log.txt", pathPackage + @"\log-" + DateTime.Now.Second + ".txt");

            }
            else
            if (mdd.Cpackages[0].NamePlatform.ToLower() == "windows")
            {
                MoveWindowsPackage(mdd, PathDirectory, pathPackage);
                _loggerService.ClosingLogger();
                File.Move(_pathPackageDirectory + @"\log\log.txt", pathPackage + @"\log-" + DateTime.Now.Second + ".txt");
            }

            _eventAggregator.GetEvent<IsBuildableEvent>().Publish(false);
        }
        #endregion

        #region MovePackage


        private void MoveAndroidPackage(ModeDeploiment mdd, string PathDirectory, string PathPackageDirectory)
        {
            MoveFiledPackage(PathDirectory + mdd.Cpackages[0].Path_appli_generate, PathPackageDirectory + "\\android-release_" + mdd.VersionIdent + "-" + mdd.ModeName.ToLower() + ".apk");
            File.Create(PathPackageDirectory + "/" + _versionCode + ".txt");
            _cmdCordovaService.CMDExecute(_config.Aapt, " aapt.exe dump badging " + PathPackageDirectory + "\\android-release_" + mdd.VersionIdent + "-" + mdd.ModeName.ToLower() + ".apk", false);
        }


        private void MoveWindowsPackage(ModeDeploiment mdd, string PathDirectory, string PathPackageDirectory)
        {
            var repositoryWindowsPackageInSolution = PathDirectory + mdd.Cpackages[0].Path_appli_generate + mdd.VersionName + "_x86_Test";
            var fileappxupload = "\\CordovaApp.Windows_" + mdd.VersionName + "_x86.appxupload";
            MoveDirectoryAndFilePackage(repositoryWindowsPackageInSolution,
                                        PathPackageDirectory + "\\CordovaApp.Windows_" + mdd.VersionName + "_x86_Test",
                                        PathDirectory + mdd.Cpackages[0].Path_appli_generate + mdd.VersionName + "_x86.appxupload",
                                        PathPackageDirectory + fileappxupload);
        }


        private void MoveFiledPackage(string PathFileStart, string PathFileEnd)
        {
            try
            {
                File.Move(PathFileStart, PathFileEnd);
                _consoleService.ConsoleAddText("Fichier déplacer: " + PathFileEnd, 3);
                _loggerService.AddLog(3, "Fichier déplacer: " + PathFileEnd);
            }
            catch (IOException ioe)
            {
                _consoleService.ConsoleAddText("MoveFiledPackage==>" + ioe.ToString(), 2);
                _loggerService.AddLog(2, "MoveFiledPackage==>" + ioe.ToString());
            }

        }

        private void MoveDirectoryAndFilePackage(string PathDiectoryStart, string PathDirectoryEnd, string PathFileStart, string PathFileEnd)
        {
            try
            {
                MoveFiledPackage(PathFileStart, PathFileEnd);
                Directory.Move(PathDiectoryStart, PathDirectoryEnd);
                _consoleService.ConsoleAddText("dossier déplacer: " + PathDirectoryEnd, 3);
                _loggerService.AddLog(3, "dossier déplacer: " + PathDirectoryEnd);
            }
            catch (IOException ioe)
            {
                _consoleService.ConsoleAddText("MoveDirectoryAndFilePackage==>" + ioe.ToString(), 2);
                _loggerService.AddLog(2, "MoveDirectoryAndFilePackage==>" + ioe.ToString());
            }
        }
        #endregion


        #region RestaurationOriginFiles

        private void RestaurationOriginFiles(string PathDirectory, ModeDeploiment dmD)
        {
            if (dmD.Cpackages[0].NamePlatform == "android")
            {
                var ConfigXml = XDocument.Load(Path.Combine(PathDirectory + "/OldConfig" + "/Old.config.xml"));
                var widget = ConfigXml.Elements().SingleOrDefault();
                var android_versionCode = widget.Attributes("android-versionCode").SingleOrDefault();
                if (dmD.VersionCode == "000000")
                {
                    android_versionCode.Value = (int.Parse(android_versionCode.Value) + 1).ToString();
                }
                else
                {
                    android_versionCode.Value = dmD.VersionCode;
                }
                _versionCode = android_versionCode.Value;
                ConfigXml.Save(Path.Combine(PathDirectory + "/OldConfig" + "/Old.config.xml"));
            }
            _backupFile.RemoveOldFile(_config.PATH_CONFIG_XML, "config.xml", PathDirectory);
            _backupFile.RemoveOldFile(_config.PATH_CONFIG_CONSTANT_JS, "config.constant.js", PathDirectory);
            _backupFile.RemoveOldFile(_config.PATH_NEXWORD_MODULE_JS, "nexworld.module.js", PathDirectory);
        }

        #endregion
    }
}
