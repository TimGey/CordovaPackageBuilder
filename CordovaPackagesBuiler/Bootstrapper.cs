using CordovaPackagesBuiler.Views;
using System.Windows;
using Prism.Modularity;
using Microsoft.Practices.Unity;
using Prism.Unity;
using CordovaPackagesBuiler.Services;

namespace CordovaPackagesBuiler
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureModuleCatalog()
        {
            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(YOUR_MODULE));
        }
        protected override void ConfigureContainer()
        {
            // injection de dépendance
            base.ConfigureContainer();
            Container.RegisterType<IConfigurationService, ConfigurationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IConsoleService, ConsoleService>();
            Container.RegisterType<IBackupFile, BackupFile>();
            Container.RegisterType<ICmdCordovaService, CmdCordovaService>();
            Container.RegisterType<IUpdateFileService, UpdateFileService>();
            Container.RegisterType<ISelectPathDirectoryService, SelectPathDiectoryService>();
            Container.RegisterType<IModeDeploimentService, ModeDeploimentService>();
            Container.RegisterType<IGeneratedPackageService, GeneratedPackagesService>();
            Container.RegisterType<ILoggerService, LoggerService>();
            Container.RegisterType<IControleInputService, ControleInputService>();
        }
    }
}
