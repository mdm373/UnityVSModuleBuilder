using System;

namespace UnityVSModuleCommon
{
    public class Logger
    {
        private static LoggingService service;

        public static void SetService(LoggingService aService)
        {
            service = aService;
        }
        public static void Log(String message){
            DefaultInitService();
            service.Log(message);
        }

        public static void LogError(String message)
        {
            DefaultInitService();
            service.LogError(message);
        }

        public static void LogError(String message, Exception e)
        {
            DefaultInitService();
            service.LogError(message, e);
        }

        private static void DefaultInitService()
        {
            if (service == null)
            {
                service = new ConsoleLoggingService();
            }
        }
    }
}
