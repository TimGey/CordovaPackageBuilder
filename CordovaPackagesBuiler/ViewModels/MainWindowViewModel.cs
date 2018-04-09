using CordovaPackagesBuiler.Entyties;
using CordovaPackagesBuiler.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using Prism.Events;
using CordovaPackagesBuiler.Events;
using System.Text;

namespace CordovaPackagesBuiler.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {


        #region Properties


        public string PathFinalDirectory
        {
            get { return _pathFinalDirectory; }
            set { SetProperty(ref _pathFinalDirectory, value); }
        }
        private string _pathFinalDirectory;


        public Config Config
        {
            get { return _config; }
            set { SetProperty(ref _config, value); }
        }
        private Config _config;

        private string _title = "Cordova Packages Builder";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        [Required]
        [RegularExpression(@"^[\d]+\.[\d]+\.[\d]+\.[\d]+$")]
        //  ex: 1.4.1.5 match ou 50.45.84.64545
        public string VersionName
        {
            get { return _versionName; }
            set { SetProperty(ref _versionName, value); }
        }
        private string _versionName;

        [Required]
        [RegularExpression(@"^[\d]+[\d]+[\d]+[\d]+$")]
        public string VersionCode
        {
            get { return _versionCode; }
            set { SetProperty(ref _versionCode, value); }
        }
        private string _versionCode;

        public string PathDirectory
        {
            get { return _pathDirectory; }
            set { SetProperty(ref _pathDirectory, value); }
        }
        private string _pathDirectory;


        public string AffichageConsole
        {
            get { return _affichageConsole; }
            set { SetProperty(ref _affichageConsole, value); }
        }
        private string _affichageConsole;

        private StringBuilder strb = new StringBuilder();

        public bool Prod
        {
            get { return _prod; }
            set { SetProperty(ref _prod, value); }
        }
        private bool _prod = false;

        public bool Preprod
        {
            get { return _preprod; }
            set { SetProperty(ref _preprod, value); }
        }
        private bool _preprod = false;

        public bool Android
        {
            get { return _android; }
            set { SetProperty(ref _android, value); }
        }
        private bool _android = false;

        public bool Windows
        {
            get { return _windows; }
            set { SetProperty(ref _windows, value); }
        }
        private bool _windows = false;

        public ModeDeploiment Mdplt
        {
            get { return _mdptl; }
            set { SetProperty(ref _mdptl, value); }
        }
        private ModeDeploiment _mdptl;

        public Package package
        {
            get { return _package; }
            set { SetProperty(ref _package, value); }
        }
        private Package _package = new Package();


        public bool FilesFind
        {
            get { return _filesfind; }
            set { SetProperty(ref _filesfind, value); }
        }
        private bool _filesfind;

        public bool IsBusy
        {
            get { return _isbusy; }
            set { SetProperty(ref _isbusy, value); }
        }
        private bool _isbusy = false;


        private string[] Tpaths;

        private readonly IConfigurationService _configurationService;
        private readonly IConsoleService _consoleService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISelectPathDirectoryService _selectPathDirectoryService;
        private readonly IGeneratedPackageService _generatedPackageService;
        private readonly IControleInputService _controleInputService;
        #endregion

        #region Constructor
        public MainWindowViewModel(
            IConfigurationService configuationService,
            IConsoleService consoleService,
            IEventAggregator eventAggregator,
            ISelectPathDirectoryService selectPathDirectoryService,
            IGeneratedPackageService generatedPackageService,
            IControleInputService controleInputService)
        {
            _configurationService = configuationService;
            _consoleService = consoleService;
            _eventAggregator = eventAggregator;
            _selectPathDirectoryService = selectPathDirectoryService;
            _generatedPackageService = generatedPackageService;
            _controleInputService = controleInputService;
            Config = _configurationService.GetConfig();
            PathDirectory = "Chemin de la solution";
            PathFinalDirectory = "Chemin de destination des packages générés";
            FilesFind = false;
            Tpaths = new string[] { Config.PATH_CONFIG_XML, Config.PATH_CONFIG_CONSTANT_JS, Config.PATH_NEXWORD_MODULE_JS };
            if (!_eventAggregator.GetEvent<MessageEvent>().Contains(OnMessageRecevied))
                _eventAggregator.GetEvent<MessageEvent>().Subscribe(OnMessageRecevied, false);
            if (!_eventAggregator.GetEvent<ClearConsoleEvent>().Contains(OnClearConsole))
                _eventAggregator.GetEvent<ClearConsoleEvent>().Subscribe(OnClearConsole, false);
            if (!_eventAggregator.GetEvent<PathFindEvent>().Contains(OnFileFind))
                _eventAggregator.GetEvent<PathFindEvent>().Subscribe(OnFileFind, false);
            if (!_eventAggregator.GetEvent<IsBuildableEvent>().Contains(OnBusyable))
                _eventAggregator.GetEvent<IsBuildableEvent>().Subscribe(OnBusyable, false);

        }
        #endregion

        #region Eventconsole
        private void OnMessageRecevied(string message)
        {
            // AffichageConsole.Append(message);
            strb.Insert(0, message, 1);
            AffichageConsole = strb.ToString();
        }

        private void OnClearConsole()
        {
            strb.Clear();
        }

        private void OnFileFind(bool find)
        {
            FilesFind = find;
        }

        private void OnBusyable(bool busy)
        {
            IsBusy = busy;
        }

        #endregion

        #region Commandes

        #region SearchSolution
        public DelegateCommand SearchSolution => new DelegateCommand(selectpath, () => { return true; });

        private void selectpath()
        {
            PathDirectory = _selectPathDirectoryService.SelectPath(Tpaths);
        }
        #endregion

        #region SelectFinalDirectoryPackages

        public DelegateCommand SelectFinalDirectoryPackages => new DelegateCommand(SelectFinalDirectory, () => { return true; });

        private void SelectFinalDirectory()
        {
            PathFinalDirectory = _selectPathDirectoryService.SelectFolder();
        }

        #endregion

        #region GeneratePackage
        public DelegateCommand GeneratePackage => new DelegateCommand(SetGeneratePackage, () => { return FilesFind; }).ObservesProperty(() => FilesFind);

        private void SetGeneratePackage()
        {

            _consoleService.clearConsole();

            #region ControleSurface
            //------choix du mode de déploiment------//
            var deployment = "";
            if (Prod)
            {
                deployment = "prod";
            }
            else
            if (Preprod)
            {
                deployment = "preprod";
            }
            else
            {
                _consoleService.ConsoleAddText("aucun déploiment coché", 1);
            }
            //------fin du choix du mode de déploiment------//

            //------choix de la platform------//
            if (deployment != "")
            {
                var platform = "";
                if (Android)
                {
                    platform = "android";
                }
                else
                 if (Windows)
                {
                    platform = "windows";
                }
                else
                {
                    _consoleService.ConsoleAddText("aucun platform cochée", 1);
                }
                //------fin du choix de la platform------//
                #endregion
                //-----appel au service------//
                if (platform != "")
                {
                    if (_controleInputService.IsNumberCode(VersionName) && _controleInputService.IsNumberVersion(_versionCode))
                    {
                        _generatedPackageService.StartGeneratedPakage(platform, deployment, VersionCode, VersionName, PathDirectory, PathFinalDirectory);
                    }


                }

            }

        }
        #endregion

        #endregion
    }
}
