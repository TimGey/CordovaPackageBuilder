﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
  public interface IBackupFile
    {
        bool CreateDirectoryOldConfig(string PathDirectory);
        bool MoveFileToBackup(string pathfile, string namefile,string PathDirectory);
        bool RemoveOldFile(string pathfile, string namefile, string PathDirectory);
        bool FileIsOpennable(string pathfile);
    }
}
