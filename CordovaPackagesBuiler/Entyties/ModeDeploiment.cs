using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Entyties
{
    public class ModeDeploiment : BindableBase
    {

        public string ModeName
        {
            get { return _modeName; }
            set { SetProperty(ref _modeName, value); }
        }
        private string _modeName;

        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }
        private string _url;


        public string VersionName
        {
            get { return _versionName; }
            set { SetProperty(ref _versionName, value); }
        }
        private string _versionName;


        public string VersionIdent
        {
            get { return _versionIdent; }
            set { SetProperty(ref _versionIdent, value); }
        }
        private string _versionIdent;


        public string VersionCode
        {
            get { return _versionCode; }
            set { SetProperty(ref _versionCode, value); }
        }
        private string _versionCode;
    

        public Collection<Package> Cpackages 
        {
            get { return _cpackage ; }
            set { SetProperty(ref _cpackage, value); }
        }
        private Collection<Package> _cpackage = new Collection<Package>();

        public ModeDeploiment(string modeName)
        {
            ModeName = modeName;
        }
    }
}
