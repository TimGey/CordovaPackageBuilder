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

        public CmdCordovaService(IConsoleService consoleService, IEventAggregator eventAggregator)
        {
            _consoleService = consoleService;
            _eventAggregator = eventAggregator;
        }

        public void CMDExecute(string DirectoryPath, string CordovaCmd, bool sendFinish)
        {

            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/C "+ CordovaCmd;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.WorkingDirectory = DirectoryPath;
            cmd.Start();
            
           
            Thread thread = new Thread(()=> ReaderHandler(cmd.StandardOutput, sendFinish));
            thread.Start();
         
        }

        private void ReaderHandler(StreamReader strReader, bool sendFinish)
        {
            try
            {
                string line = "";
                while((line= strReader.ReadLine()) != null)
                {
                    _consoleService.ConsoleAddText("Console===>" + line, 0);
                }
                if (sendFinish)
                {
                    _eventAggregator.GetEvent<ThreadFinishEvent>().Publish();
                }
            }
            catch(IOException ioe)
            {
                _consoleService.ConsoleAddText("ReaderHandler---->" + ioe.ToString(), 2);
            }
        }
      
       
    }
}
