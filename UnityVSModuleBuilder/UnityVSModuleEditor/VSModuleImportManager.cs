using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    class VSModuleImportManager
    {
        private const string UNITYPACKAGE_EXTENSION = ".unitypackage";
        private const string CONFIG_MODULE_NAME = @"ModuleConfig.xml";
        private const string ASSET_FOLDER_NAME = "Assets";
        private const string EDITOR_FOLDER_NAME = "Editor";

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
                String moduleInfoPath = Path.Combine(assetRoot,EDITOR_FOLDER_NAME);
                moduleInfoPath = Path.Combine(moduleInfoPath, CONFIG_MODULE_NAME);

                String repoLocation = to.GetRepoLocation();
                
                String companyProjectPath = to.GetCompanyShortName() + Path.DirectorySeparatorChar + to.GetProjectName();
                String moduleRepoLocation = Path.Combine(repoLocation, companyProjectPath);
                Directory.CreateDirectory(moduleRepoLocation);

                String editorAssetsFolder = Path.Combine(ASSET_FOLDER_NAME, EDITOR_FOLDER_NAME);
                String editorPackageName = to.GetProjectName() + EDITOR_FOLDER_NAME;
                ExportPackage(moduleRepoLocation, ASSET_FOLDER_NAME, companyProjectPath, to.GetProjectName());
                ExportPackage(moduleRepoLocation, editorAssetsFolder, companyProjectPath, editorPackageName);
                File.Copy(moduleInfoPath, Path.Combine(moduleRepoLocation, CONFIG_MODULE_NAME), true);
            }
            catch (Exception e)
            {
                unityApi.LogError("Unexpected Exception Exporting Module To Repository. See Log For Error Details.");
                unityApi.LogException(e);
                isExported = false;
            }
            return isExported;
        }

        private void ExportPackage(String moduleRepoLocation, String assetRoot, String companyProjectPath, String packageName)
        {
            String exportPackageName = Path.Combine(moduleRepoLocation, packageName + UNITYPACKAGE_EXTENSION);
            String assetPath = Path.Combine(assetRoot, companyProjectPath);

            unityApi.ExportRootAssets(assetPath, exportPackageName);
        }
    }
}
