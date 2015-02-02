using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityVSModuleEditor;
using UnityVSModuleEditor.MiddleTier;

namespace UnityVSModuleEditor.UI
{
    internal class UnityApiImpl : UnityApi
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        public void LogException(Exception e)
        {
            Debug.LogException(e);
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        public string GetAssetFolder()
        {
            return Application.dataPath;
        }


        public void ExportRootAssets(string[] assetPaths, String exportFileName)
        {
            ExportPackageOptions options = ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse;
            AssetDatabase.ExportPackage(assetPaths, exportFileName, options);
        }

        public void ImportRootAssets(String importFileName)
        {
            AssetDatabase.ImportPackage(importFileName, false);
        }


        public void LogError(string p, Exception e)
        {
            LogError(p);
            LogException(e);
        }

        public void UpdateProjectName(string projectName)
        {
            PlayerSettings.productName = projectName;
        }

        public void UpdateCompanyName(string companyName)
        {
            PlayerSettings.companyName = companyName;
        }

        public void UpdateAndriodBundleIdentifier(string androidIdenfitifer)
        {
            PlayerSettings.bundleIdentifier = androidIdenfitifer;
        }
    }
}
