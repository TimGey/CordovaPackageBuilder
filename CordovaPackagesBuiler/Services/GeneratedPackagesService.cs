﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;

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
        private Config _config;
        #endregion

        #region constructeur
        public GeneratedPackagesService(IModeDeploimentService modeDeploimentService,
                                        IBackupFile backupFile,
                                        IConsoleService consoleService,
                                        IUpdateFileService updateFileService,
                                        ICmdCordovaService cmdCordovaService,
                                        IConfigurationService configurationService)
        {
            _modeDeploimentService = modeDeploimentService;
            _backupFile = backupFile;
            _consoleService = consoleService;
            _updateFileService = updateFileService;
            _cmdCordovaService = cmdCordovaService;
            _configurationService = configurationService;
            _config = _configurationService.GetConfig();
        }
        #endregion

        public void StartGeneratedPakage(string plateform, string deploiment, string VersionCode, string VersionName, string PathDirectory)
        {
            ModeDeploiment MdDplt = _modeDeploimentService.CreateModeDeploid(deploiment, VersionName, VersionCode);
            MdDplt = _modeDeploimentService.AddPlatform(MdDplt, plateform);
            _backupFile.CreateDirectoryOldConfig(PathDirectory);
            _consoleService.ConsoleAddText("écriture du config.xml", 0);
            if (_updateFileService.UpdateConfigXml(MdDplt, PathDirectory, _config.PATH_CONFIG_XML))
            {
                _consoleService.ConsoleAddText("fin de l'écriture config.xml", 3);
            }
            else
            {
                _consoleService.ConsoleAddText("Une erreure est survenue lors de l'écriture config.xml", 2);
            }

            _consoleService.ConsoleAddText("Copie et création d'un config.constant.js", 0);
            if (_updateFileService.CreateConfigConstantJS(MdDplt, PathDirectory, _config.PATH_CONFIG_CONSTANT_JS))
            {
                _consoleService.ConsoleAddText("fin de Copie et création d'un config.constant.js", 3);
            }
            else
            {
                _consoleService.ConsoleAddText("Une erreure est survenue lors de la Copie et création d'un config.constant.js", 2);
            }

            _consoleService.ConsoleAddText("Copie et modification du nexworld.module.js", 0);
            if (_updateFileService.UpdateNexworldModuleJs(MdDplt, PathDirectory, _config.PATH_NEXWORD_MODULE_JS))
            {
                _consoleService.ConsoleAddText("fin de Copie et modification du nexworld.module.js", 3);
            }
            else
            {
                _consoleService.ConsoleAddText("Une erreure est survenue lors de la Copie et modification du nexworld.module.js", 2);
            }

            _cmdCordovaService.GeneratePackage("android", PathDirectory, MdDplt.Cpackages[0].CordovaCmd);

            //_backupFile.RemoveOldFile(_config.PATH_CONFIG_XML, "config.xml", PathDirectory);
           // _backupFile.RemoveOldFile(_config.PATH_CONFIG_CONSTANT_JS, "config.constant.js", PathDirectory);
           // _backupFile.RemoveOldFile(_config.PATH_NEXWORD_MODULE_JS, "nexworld.module.js", PathDirectory);


        }
    }
}