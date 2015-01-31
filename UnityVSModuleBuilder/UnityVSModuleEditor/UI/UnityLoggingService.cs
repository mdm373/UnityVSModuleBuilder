using System;
using UnityEngine;
using UnityVSModuleCommon;

namespace UnityVSModuleEditor.UI
{
    internal class UnityLoggingService : LoggingService
    {

        public static readonly LoggingService INSTANCE = new UnityLoggingService();
        
        private UnityLoggingService()
        {

        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogError(string message, Exception e)
        {
            Debug.LogError(message);
            Debug.LogException(e);
        }
    }
}
