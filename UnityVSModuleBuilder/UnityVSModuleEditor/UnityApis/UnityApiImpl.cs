﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityVSModuleEditor.UnityApis
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


        public void ExportRootAssets(string assetPathname, String exportFileName)
        {
            ExportPackageOptions options = ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse;
            AssetDatabase.ExportPackage(assetPathname, exportFileName, options);
        }
    }
}
