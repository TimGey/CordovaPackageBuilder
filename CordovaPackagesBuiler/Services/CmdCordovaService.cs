using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;
using Prism.Events;
using CordovaPackagesBuiler.Events;

namespace CordovaPackagesBuiler.Services
{
    public class CmdCordovaService : ICmdCordovaService
    {
        private readonly IConsoleService _consoleService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILoggerService _loggerService;

        public CmdCordovaService(IConsoleService consoleService, IEventAggregator eventAggregator, ILoggerService loggerService)
        {
            _consoleService = consoleService;
            _eventAggregator = eventAggregator;
            _loggerService = loggerService;
        }

        public void CMDExecute(string DirectoryPath, string CordovaCmd, bool sendFinish)
        {

            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/C " + CordovaCmd;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.WorkingDirectory = DirectoryPath;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.Start();

            Thread therror = new Thread(() => ReaderHandler(cmd ,cmd.StandardError, false));
            therror.Start();

            Thread thread = new Thread(() => ReaderHandler(cmd, cmd.StandardOutput, sendFinish));
            thread.Start();

        }

        private void ReaderHandler(Process p ,StreamReader strReader, bool sendFinish)
        {
            try
            {
                string line = "";
                while ((line = strReader.ReadLine()) != null)
                {
                    _consoleService.ConsoleAddText("Console===>" + line, 0);
                    _loggerService.AddLog(0, line);

                }
                if (sendFinish)
                {
                    _eventAggregator.GetEvent<CmdIsFinishEvent>().Publish();
                }
            }
            catch (IOException ioe)
            {
                _consoleService.ConsoleAddText("ReaderHandler---->" + ioe.ToString(), 2);
                _loggerService.AddLog(2, "ReaderHandler---- > " + ioe.ToString());
            }

            p.Close();
        }


    }
}
