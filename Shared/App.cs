using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public static class App
    {
        public static Config Config = new Config();
        public static NLog.Logger Logger = LogManager.GetLogger("Logger");
    }
}