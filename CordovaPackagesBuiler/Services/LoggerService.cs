using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace CordovaPackagesBuiler.Services
{
    public class LoggerService : ILoggerService
    {


        public void AddLog(int typeLog, string msg)
        {

            switch (typeLog)
            {
                case 0:
                    Log.Information(msg);
                    break;

                case 1:
                    Log.Error(msg);
                    break;

                case 2:
                    Log.Warning(msg);
                    break;

                case 3:
                    Log.Verbose(msg);
                    break;

                default:
                    Log.Debug(msg);
                    break;
            }


        }

        public void ClosingLogger()
        {
            Log.CloseAndFlush();
        }

        public void SetPathLog(string path)
        {
            
            Log.Logger = new LoggerConfiguration()
                   .MinimumLevel.Debug()
                   .WriteTo.File(path + @"\Log\log.txt")
                   .CreateLogger();
        }
    }
}
