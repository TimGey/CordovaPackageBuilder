using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;
using Prism.Events;
using CordovaPackagesBuiler.Events;


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
        private Config _config;
        private string _pathdirectory;
        private string _pathPackageDirectory;
        private ModeDeploiment _mdDplt;

        #endregion

        #region constructeur
        public GeneratedPackagesService(IModeDeploimentService modeDeploimentService,
                                        IBackupFile backupFile,
                                        IConsoleService consoleService,
                                        IUpdateFileService updateFileService,
                                        ICmdCordovaService cmdCordovaService,
                                        IConfigurationService configurationService,
                                        ISelectPathDirectoryService selectPathDirectoryService,
                                        IEventAggregator eventAggregator)
        {
            _modeDeploimentService = modeDeploimentService;
            _backupFile = backupFile;
            _consoleService = consoleService;
            _updateFileService = updateFileService;
            _cmdCordovaService = cmdCordovaService;
            _configurationService = configurationService;
            _selectPathDirectoryService = selectPathDirectoryService;
            _eventAggregator = eventAggregator;
            _config = _configurationService.GetConfig();
            if (!_eventAggregator.GetEvent<ThreadFinishEvent>().Contains(OnFinishRecevied))
                _eventAggregator.GetEvent<ThreadFinishEvent>().Subscribe(OnFinishRecevied, false);
        }
        #endregion

        #region Event

        private void OnFinishRecevied()
        {
            OnCmdFinish();
        }

        #endregion

        public void StartGeneratedPakage(string plateform, string deploiment, string VersionCode, string VersionName, string PathDirectory, string PathPackageDirectory)
        {
            _pathdirectory = PathDirectory;
            _pathPackageDirectory = PathPackageDirectory;

            //--vérification des fichers si lecture seul--//
            if (!ReadOnlyFile(PathDirectory, new string[] { _config.PATH_CONFIG_XML, _config.PATH_CONFIG_CONSTANT_JS, _config.PATH_NEXWORD_MODULE_JS }))
            {
                //--instanciation d'un ModeDeploiment && ajout d'une platform--//
                ModeDeploiment MdDplt = _modeDeploimentService.CreateModeDeploid(deploiment, VersionName, VersionCode);
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

        private void OnCmdFinish()
        {
            //--restauration des fichiers d'oigine--//
            RestaurationOriginFiles(_pathdirectory);

            if (_pathPackageDirectory != "Chemin de destination des packages générés")
            {

                MovePackage(_mdDplt, _pathdirectory, _pathPackageDirectory);
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
            var nameFolderpackage = @"\" + DateTime.Now.Date.ToString("yyyyMMdd") + "_FONCIA_VISITE_" + mdd.ModeName.ToUpper() + "_" + mdd.VersionCode;
            string[] tFolder = new string[] { @"\LivraisonFoncia", @"\Version", @"\" + mdd.Cpackages[0].NamePlatform.ToUpper(), nameFolderpackage };

            _backupFile.CreateDirectory(PathPackageDirectory, tFolder);

            string pathPackage = PathPackageDirectory + string.Join("", tFolder);

            if (mdd.Cpackages[0].NamePlatform.ToLower() == "android")
            {
                MoveAndroidPackage(mdd, PathDirectory, pathPackage);
            }
            else
            if (mdd.Cpackages[0].NamePlatform.ToLower() == "windows")
            {
                MoveWindowsPackage(mdd, PathDirectory, pathPackage);
            }

        }
        #endregion

        #region MovePackage


        private void MoveAndroidPackage(ModeDeploiment mdd, string PathDirectory, string PathPackageDirectory)
        {
            MoveFiledPackage(PathDirectory + mdd.Cpackages[0].Path_appli_generate, PathPackageDirectory + "\\android-release_" + mdd.VersionCode + "-" + mdd.ModeName.ToLower() + ".apk");
            _cmdCordovaService.CMDExecute(_config.Aapt, " aapt.exe dump badging "+ PathPackageDirectory + "\\android-release_" + mdd.VersionCode + "-" + mdd.ModeName.ToLower() + ".apk", false);
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
            }
            catch (IOException ioe)
            {
                _consoleService.ConsoleAddText("MoveFiledPackage==>" + ioe.ToString(), 2);
            }

        }

        private void MoveDirectoryAndFilePackage(string PathDiectoryStart, string PathDirectoryEnd, string PathFileStart, string PathFileEnd)
        {
            try
            {
                MoveFiledPackage(PathFileStart, PathFileEnd);
                Directory.Move(PathDiectoryStart, PathDirectoryEnd);
                _consoleService.ConsoleAddText("dossier déplacer: " + PathDirectoryEnd, 3);
            }
            catch (IOException ioe)
            {
                _consoleService.ConsoleAddText("MoveDirectoryAndFilePackage==>" + ioe.ToString(), 2);
            }
        }
        #endregion


        #region RestaurationOriginFiles

        private void RestaurationOriginFiles(string PathDirectory)
        {
            _backupFile.RemoveOldFile(_config.PATH_CONFIG_XML, "config.xml", PathDirectory);
            _backupFile.RemoveOldFile(_config.PATH_CONFIG_CONSTANT_JS, "config.constant.js", PathDirectory);
            _backupFile.RemoveOldFile(_config.PATH_NEXWORD_MODULE_JS, "nexworld.module.js", PathDirectory);
        }

        #endregion
    }
}
