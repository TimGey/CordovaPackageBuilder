using CordovaPackagesBuiler.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
    public class ConsoleService : IConsoleService
    {
        private readonly IEventAggregator _eventAggregator;

        public ConsoleService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void clearConsole()
        {
            _eventAggregator.GetEvent<ClearConsoleEvent>().Publish();
        }

        public void ConsoleAddText(string message, int cdmsg)
        {
            string[] codeMessage = new string[] { "Info", "Error", "Warning", "Success" };
           
            _eventAggregator.GetEvent<MessageEvent>().Publish(codeMessage[cdmsg] + "---" + DateTime.Now + "---" + message + "\n");
        }
    }
}
