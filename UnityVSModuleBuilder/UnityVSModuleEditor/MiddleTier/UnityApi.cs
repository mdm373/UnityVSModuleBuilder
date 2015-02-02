using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface UnityApi
    {
        void Log(String message);
        void LogError(String message);
        void LogError(string p, Exception e);
        void LogException(Exception e);
        void LogWarning(String message);
        String GetAssetFolder();
        void ExportRootAssets(string[] assetPathname, String exportFileName);
        void ImportRootAssets(String importFileName);
        void UpdateProjectName(string projectName);
        void UpdateCompanyName(string comapnyName);
        void UpdateAndriodBundleIdentifier(string androidIdenfitifer);
    }
}
