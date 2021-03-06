﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Entyties
{
    public class Package : BindableBase
    {
        #region Properties

        public string NamePlatform
        {
            get { return _namePlatform; }
            set { SetProperty(ref _namePlatform, value); }
        }
        private string _namePlatform;

        public string NameApli
        {
            get { return _nameApli; }
            set { SetProperty(ref _nameApli, value); }
        }
        private string _nameApli;

        public string NamePackage
        {
            get { return _namePackage; }
            set { SetProperty(ref _namePackage, value); }
        }
        private string _namePackage;


        public string DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }
        private string _deviceType;

        public string CordovaCmd
        {
            get { return _cordovaCmd; }
            set { SetProperty(ref _cordovaCmd, value); }
        }
        private string _cordovaCmd;


        public string Path_appli_generate
        {
            get { return _path_appli_generate; }
            set { SetProperty(ref _path_appli_generate, value); }
        }
        private string _path_appli_generate;

        #endregion

        #region Constructeurs
        public Package()
        {
        }

        public Package(string namePlatform)
        {
            NamePlatform = namePlatform;
        }

        public Package(string namePlatform, string nameApli, string namePackage, string deviceType, string cordovaCmd ,string path_appli_generate)
        {
            NamePackage = namePackage;
            NameApli = nameApli;
            DeviceType = deviceType;
            NamePlatform = namePlatform;
            CordovaCmd = cordovaCmd;
            Path_appli_generate = path_appli_generate;
        }
        #endregion
    }
}
