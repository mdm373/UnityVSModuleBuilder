using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon
{
    class LoggingServiceFactory
    {
        private LoggingServiceFactory(){}
        public LoggingService GetConsoleLoggingService()
        {
            return new LoggingServiceConsole();
        }
    }
}
