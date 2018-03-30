using CordovaPackagesBuiler.Entyties;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
    public class BackupFile : IBackupFile
    {
        #region Properties
        private Config Config
        {
            get;
            set;
        }
        private readonly IConfigurationService _configurationService;
        private readonly IConsoleService _consoleService;
        #endregion

        #region Constructor
        public BackupFile(IConfigurationService configurationService, IConsoleService consoleService)
        {
            _configurationService = configurationService;
            _consoleService = consoleService;
            Config = _configurationService.GetConfig();
        }
        #endregion

        public bool CreateDirectory(string PathDirectory, string[] tDirectorys)
        {
            bool result = false;
            try
            {
                for (var i = 0; i<tDirectorys.Length; i++)
                {
                    if (i != 0)
                    {
                        PathDirectory += tDirectorys[i-1];
                    }
                    if (!Directory.Exists(PathDirectory + tDirectorys[i]))
                    {
                        _consoleService.ConsoleAddText("creation du dossier " + tDirectorys[i] + " :" + PathDirectory, 0);
                        Directory.CreateDirectory(PathDirectory + tDirectorys[i]);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _consoleService.ConsoleAddText("CreateDirectory===>" + ex.ToString(), 1);
                result = false;
            }

            return result;
        }

        public bool MoveFileToBackup(string pathfile, string namefile, string PathDirectory)
        {
            if (File.Exists(PathDirectory + @"\OldConfig" + @"\Old." + namefile))
            {
                File.Delete(PathDirectory + @"\OldConfig" + @"\Old." + namefile);
            }
            _consoleService.ConsoleAddText("copie du fichier :" + namefile + " dans :" + PathDirectory + @"\Old" + namefile, 0);
            File.Copy(pathfile, PathDirectory + @"\OldConfig" + @"\Old." + namefile);

            return true;
        }

        public bool RemoveOldFile(string pathfile, string namefile, string PathDirectory)
        {
            bool result = false;
            if (File.Exists(PathDirectory + @"\OldConfig" + @"\Old." + namefile))
            {
                if (File.Exists(PathDirectory + pathfile))
                {
                    DeleteFile(pathfile, namefile, PathDirectory);
                    MoveFile(pathfile, namefile, PathDirectory);
                    result = true;
                }
                else
                {
                    MoveFile(pathfile, namefile, PathDirectory);
                    result = true;
                }
            }
            return result;
        }

        private void MoveFile(string pathfile, string namefile, string PathDirectory)
        {
            try
            {
                File.Move(PathDirectory + @"\OldConfig" + @"\Old." + namefile, PathDirectory + pathfile);
            }
            catch (Exception ex)
            {
                _consoleService.ConsoleAddText("MoveFile====>" + namefile + "  " + ex.ToString(), 2);
            }
        }

        private void DeleteFile(string pathfile, string namefile, string PathDirectory)
        {
            try
            {
                File.Delete(PathDirectory + pathfile);
            }
            catch (Exception ex)
            {
                _consoleService.ConsoleAddText("DeleteFile====>" + namefile + ex.ToString(), 2);
            }
        }

        public bool FileIsOpennable(string pathfile)
        {
            FileInfo fileInfos = new FileInfo(pathfile);
            return fileInfos.IsReadOnly;
        }
    }
}
