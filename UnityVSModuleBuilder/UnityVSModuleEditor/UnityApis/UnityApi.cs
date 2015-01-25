using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleEditor.UnityApis
{
    public interface UnityApi
    {
        void Log(String message);
        void LogError(String message);
        void LogException(Exception e);
        void LogWarning(String message);
        String GetAssetFolder();
    }
}
