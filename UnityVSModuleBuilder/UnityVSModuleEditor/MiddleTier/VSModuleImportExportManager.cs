﻿using System;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;
using System.Collections.Generic;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleImportExportManager
    {
        bool ExportModule(VSModuleSettingsTO to);
        bool ImportModule(string companyShortName, string projectName, VSModuleSettingsTO settingsTO);
    }

    internal class VSModuleImportExportManagerImpl : VSModuleImportExportManager
    {
        private readonly UnityApi unityApi;
        private readonly FileSystemController fsController;
        
        
        public VSModuleImportExportManagerImpl(UnityApi unityApi, FileSystemController fsController)
        {
            this.unityApi = unityApi;
            this.fsController = fsController;
        }

        public bool ExportModule(VSModuleSettingsTO to)
        {
            bool isExported = false;
            try
            {
                String projectRoot = unityApi.GetProjectFolder();
                String moduleInfoPath = Path.Combine(projectRoot, VSModuleConstants.CONFIG_FILE_ASSET_LOCATION);
                FileEntry moduleInfoFile = fsController.GetExistingFile(moduleInfoPath);
                
                String repoLocation = to.GetRepoLocation();
                
                String companyProjectPath = to.GetCompanyShortName() + Path.DirectorySeparatorChar + to.GetProjectName();
                String moduleRepoLocation = GetRepoLocation(to.GetCompanyShortName(), to.GetProjectName(), to.GetRepoLocation());
                if (fsController.DoCreateDirectory(moduleRepoLocation))
                {
                    ExportPackage(moduleRepoLocation, companyProjectPath, to.GetProjectName());
                    fsController.DoFileCopy(moduleInfoFile, moduleRepoLocation);
                    isExported = true;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected Exception Exporting Module To Repository. See Log For Error Details.", e);
                isExported = false;
            }
            return isExported;
        }

        private string GetRepoLocation(String companyShortName, String projectName, String repoLocation)
        {
            String companyProjectPath = Path.Combine(companyShortName, projectName);
            return Path.Combine(repoLocation, companyProjectPath);
        }

        private void ExportPackage(String moduleRepoLocation, String companyProjectPath, String packageName)
        {
            String exportPackageName = Path.Combine(moduleRepoLocation, packageName + VSModuleConstants.UNITYPACKAGE_EXTENSION);
            String managedCodePath = Path.Combine(VSModuleConstants.ASSET_FOLDER_NAME, VSModuleConstants.MANAGED_CODE_FOLDER_NAME);
            String assetPath = Path.Combine(VSModuleConstants.ASSET_FOLDER_NAME, companyProjectPath);

            String editorManagedCodePath = Path.Combine(VSModuleConstants.ASSET_FOLDER_NAME, VSModuleConstants.EDITOR_FOLDER_NAME);
            editorManagedCodePath = Path.Combine(editorManagedCodePath, VSModuleConstants.MANAGED_CODE_FOLDER_NAME);
            String editorPath = Path.Combine(VSModuleConstants.ASSET_FOLDER_NAME, VSModuleConstants.EDITOR_FOLDER_NAME);
            editorPath = Path.Combine(editorPath, companyProjectPath);
            String pluginPath = Path.Combine(VSModuleConstants.ASSET_FOLDER_NAME, VSModuleConstants.PLUGIN_FOLDER_NAME);
            String[] assetPaths = new String[] { editorManagedCodePath, managedCodePath, assetPath, editorPath, pluginPath };
            List<String> verifiedAssetPaths = new List<string>();
            String projectRoot = unityApi.GetProjectFolder();
            foreach(String unverifiedPath in assetPaths)
            {
                String assetItemFullPath = Path.Combine(projectRoot, unverifiedPath);
                if (fsController.GetExistingFileOrDirectory(assetItemFullPath) != null)
                {
                    verifiedAssetPaths.Add(unverifiedPath);
                }
            }
            if (verifiedAssetPaths.Count > 0)
            {
                unityApi.ExportRootAssets(verifiedAssetPaths.ToArray(), exportPackageName);
            }
        }


        public bool ImportModule(string companyShortName, string projectName, VSModuleSettingsTO settingsTO)
        {
            bool isImported = false;
            try
            {
                String repoLocation = GetRepoLocation(companyShortName, projectName, settingsTO.GetRepoLocation());
                String import = Path.Combine(repoLocation, projectName + VSModuleConstants.UNITYPACKAGE_EXTENSION);
                FileEntry importFile = fsController.GetExistingFile(import);
                if (importFile != null && importFile.IsPresent())
                {
                    unityApi.ImportRootAssets(import);
                    isImported = true;
                }
                else
                {
                    Logger.LogError("Failed To Import Dependency Module Assets at '" + import + "'. File Not Found In Repo.");
                }   
            }
            catch (Exception e)
            {
                isImported = false;
                Logger.LogError("Unexpected Exception Importing Module From Repository. See Log For Error Details.", e);
            }
            return isImported;
        }
    }
}
