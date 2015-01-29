using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleImportManager
    {
        private const string UNITYPACKAGE_EXTENSION = ".unitypackage";
        private const string ASSET_FOLDER_NAME = "Assets";
        private const string EDITOR_FOLDER_NAME = "Editor";
        private const string MANAGED_CODE_FOLDER_NAME = "ManagedCode";

        private readonly UnityApi unityApi;
        
        public VSModuleImportManager(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public bool ExportModule(VSModuleSettingsTO to)
        {
            bool isExported = true;
            try
            {
                String assetRoot = unityApi.GetAssetFolder();
                String moduleInfoPath = Path.Combine(assetRoot, VSModuleConstants.CONFIG_FILE_ASSET_LOCATION);
                
                String repoLocation = to.GetRepoLocation();
                
                String companyProjectPath = to.GetCompanyShortName() + Path.DirectorySeparatorChar + to.GetProjectName();
                String moduleRepoLocation = GetRepoLocation(to.GetCompanyShortName(), to.GetProjectName(), to.GetRepoLocation());
                Directory.CreateDirectory(moduleRepoLocation);

                ExportPackage(moduleRepoLocation, companyProjectPath, to.GetProjectName());
                File.Copy(moduleInfoPath, Path.Combine(moduleRepoLocation, VSModuleConstants.CONFIG_FILE_NAME), true);
            }
            catch (Exception e)
            {
                unityApi.LogError("Unexpected Exception Exporting Module To Repository. See Log For Error Details.");
                unityApi.LogException(e);
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
            String exportPackageName = Path.Combine(moduleRepoLocation, packageName + UNITYPACKAGE_EXTENSION);
            String managedCodePath = Path.Combine(ASSET_FOLDER_NAME, MANAGED_CODE_FOLDER_NAME);
            String assetPath = Path.Combine(ASSET_FOLDER_NAME, companyProjectPath);

            String editorManagedCodePath = Path.Combine(ASSET_FOLDER_NAME, EDITOR_FOLDER_NAME);
            editorManagedCodePath = Path.Combine(editorManagedCodePath, MANAGED_CODE_FOLDER_NAME);
            String editorPath = Path.Combine(ASSET_FOLDER_NAME, EDITOR_FOLDER_NAME);
            editorPath = Path.Combine(editorPath, companyProjectPath);

            String[] assetPaths = new String[] { editorManagedCodePath, managedCodePath, assetPath, editorPath };
            unityApi.ExportRootAssets(assetPaths, exportPackageName);
        }


        internal bool ImportModule(string companyShortName, string projectName, VSModuleSettingsTO settingsTO)
        {
            bool isImported = true;
            try
            {
                String repoLocation = GetRepoLocation(companyShortName, projectName, settingsTO.GetRepoLocation());
                String import = Path.Combine(repoLocation, projectName + UNITYPACKAGE_EXTENSION);
                unityApi.ImportRootAssets(import);
            }
            catch (Exception e)
            {
                isImported = false;
                unityApi.LogError("Unexpected Exception Importing Module From Repository. See Log For Error Details.", e);
            }
            return isImported;
        }
    }
}
