using System;

namespace UnityVSModuleCommon
{
    public interface LoggingService
    {
        void Log(string message);

        void LogError(string message);

        void LogError(string message, Exception e);
    }
}
