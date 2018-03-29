using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;

namespace CordovaPackagesBuiler.Services
{
    public class CmdCordovaService : ICmdCordovaService
    {
        private readonly IConsoleService _consoleService;

        public CmdCordovaService(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public void CMDExecute(string DirectoryPath, string CordovaCmd)
        {

            var cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.Arguments = "/C "+ CordovaCmd;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.WorkingDirectory = DirectoryPath;
            cmd.Start();
           
            Thread thread = new Thread(()=> ReaderHandler(cmd.StandardOutput));
            thread.Start();
           


        }

        private void ReaderHandler(StreamReader strReader)
        {
            try
            {
                

                string line = "";
                while((line= strReader.ReadLine()) != null)
                {
                    _consoleService.ConsoleAddText("Console===>" + line, 0);
                }

            }
            catch(IOException ioe)
            {
                _consoleService.ConsoleAddText("ReaderHandler---->" + ioe.ToString(), 2);
            }
        }
      
    }
}
