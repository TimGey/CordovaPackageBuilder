using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CordovaPackagesBuiler.Entyties;

namespace CordovaPackagesBuiler.Services
{
    public class ControleInputService : IControleInputService
    {
        private readonly IConsoleService _consoleService;

        public ControleInputService(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        public bool IsNumberCode(string input)
        {
            var result = false;
            if (IsNotNullable(input))
            {
                Regex rx = new Regex(@"^(\d+.){3}\d$");
                result = rx.Match(input).Success;
                SendResult(result, input);
            }
            return result;
        }
        public bool IsNumberVersion(string input)
        {
            var result = false;
            if (IsNotNullable(input))
            {
                Regex rx = new Regex(@"^\d{4}\d*$");
                result = rx.Match(input).Success;
                SendResult(result, input);
            }
            return result;
        }


        private void SendResult(bool result, string input)
        {

            if (result == false)
            {
                _consoleService.ConsoleAddText("la valeur saissie " + input + " ne correspond pas au format attendu", 1);
            }

        }
        private bool IsNotNullable(string input)
        {
            if (input == null || input.Trim() == "")
            {
                _consoleService.ConsoleAddText("l'une des valeurs saisie est null", 2);
                return false;
            }
            return true;
        }
    }
}
