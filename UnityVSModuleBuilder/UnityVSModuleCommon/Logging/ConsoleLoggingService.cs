using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.Logging
{
    public class ConsoleLoggingService : LoggingService
    {
        public void Log(string message)
        {
            Console.Out.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.Error.WriteLine(message);
        }

        public void LogError(string message, Exception e)
        {
            LogError(message);
            Console.Error.WriteLine(e);
        }
    }
}
