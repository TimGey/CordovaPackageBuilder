using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Events;
using Microsoft.Win32;
using Prism.Events;

namespace CordovaPackagesBuiler.Services
{
    public class SelectPathDiectoryService : ISelectPathDirectoryService
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IConsoleService _consoleService;

        public SelectPathDiectoryService(IEventAggregator eventAggregator, IConsoleService consoleService)
        {
            _eventAggregator = eventAggregator;
            _consoleService = consoleService;
        }

        private bool FileExiste(string[] tPathFile, string PathDirectory)
        {
            bool find = true;
            foreach (string pathfile in tPathFile)
            {
                if (!File.Exists(PathDirectory + pathfile))
                {
                    var combine = (PathDirectory + pathfile);
                    var message = " Chemin Incorrect :\n" + pathfile;
                    _consoleService.ConsoleAddText(message, 1);
                    find = false;
                }
            }
            _eventAggregator.GetEvent<FileIsFindEvent>().Publish(find);
            return find;
        }


        public void SelectPath(string[] tPathFiles)
        {
            string PathDirectory;
            _consoleService.clearConsole();
            OpenFileDialog opfile = new OpenFileDialog();

            var result = opfile.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                PathDirectory = Path.GetDirectoryName(opfile.FileName);
                if (FileExiste(tPathFiles, PathDirectory))
                {
                    _consoleService.ConsoleAddText(" Solution correcte", 3);
                }
                _eventAggregator.GetEvent<PathEvent>().Publish(PathDirectory);
            }
        }
    }
}
