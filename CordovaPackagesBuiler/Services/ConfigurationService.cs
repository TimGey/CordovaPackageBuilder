using CordovaPackagesBuiler.Entyties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CordovaPackagesBuiler.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public Config GetConfig()
        {
            return new Config();
        }
    }
}
