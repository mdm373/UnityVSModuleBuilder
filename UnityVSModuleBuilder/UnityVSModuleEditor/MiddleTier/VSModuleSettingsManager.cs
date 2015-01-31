using System;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleSettingsManager
    {
        bool SaveModuleSettingsTO(VSModuleSettingsTO to);
        VSModuleSettingsTO RetrieveModuleSettingsTO();
    }

    internal class VSModuleSettingsManagerImpl : VSModuleSettingsManager
    {
        private readonly UnityApi unityApi;
        private readonly XmlSerializerWrapper serializer;
        private FileSystemController fsController;

        public VSModuleSettingsManagerImpl(UnityApi unityApi, XmlSerializerWrapper serializer, FileSystemController fsController)
        {
            this.unityApi = unityApi;
            this.serializer = serializer;
            this.fsController = fsController;
        }

        public bool SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            bool isSaved = true;
            try
            {
                SendToToFile(to);
            }
            catch (Exception e)
            {
                isSaved = false;
                unityApi.LogError("Could Not Save VSMoule Settings. Unexpected Error. See Exception for Details.");
                unityApi.LogException(e);
            }
            return isSaved;
        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            VSModuleSettingsTO to = null;
            try
            {
                to = GetTOFromFile();
            }
            catch (Exception e)
            {
                Logger.LogError("Could Not Populate VSModule Settings. Unexpected Error. See Exception for Details.");
                unityApi.LogException(e);
                to = GetDefaultTO();
            }
            finally
            {
                if (to == null)
                {
                    Logger.Log("Using Default VSModule Settings. See log for error details loading settings from file.");
                    to = GetDefaultTO();
                }
            }
            return to;
        }

        

        private void SendToToFile(VSModuleSettingsTO to)
        {
            FileEntry info = GetSettingsFileInfo(true);
            if(info != null)
            {
                VSModuleSettingsXmlModel model = TranslateTOtoModel(to);
                serializer.SerializeToFile<VSModuleSettingsXmlModel>(info, model);
            }
            
        }

        private VSModuleSettingsTO GetTOFromFile()
        {
            VSModuleSettingsTO to = null;
            FileEntry info = GetSettingsFileInfo(false);
            if (info != null && info.IsPresent())
            {
                VSModuleSettingsXmlModel model = serializer.GetDeserialized<VSModuleSettingsXmlModel>(info);
                if (model != null)
                {
                    to = TranslateModeltoTO(model);
                }
            }
            else
            {
                Logger.LogError("Could not initialize VSModule settings. Config File Not Found From Asset Root. '" + VSModuleConstants.CONFIG_FILE_ASSET_LOCATION + '\'');
            }
            return to;
        }

        private VSModuleSettingsTO TranslateModeltoTO(VSModuleSettingsXmlModel model)
        {
            VSModuleSettingsTO.Builder builder = new VSModuleSettingsTO.Builder();
            builder.ProjectName = model.projectName;
            builder.CompanyName = model.companyName;
            builder.CompanyShortName = model.companyShortName;
            builder.RepoLocation = model.repoLocation;
            builder.UnityInstallLocation = model.unityInstallLocation;
            return builder.Build();
        }

        private VSModuleSettingsXmlModel TranslateTOtoModel(VSModuleSettingsTO to)
        {
            VSModuleSettingsXmlModel model = new VSModuleSettingsXmlModel();
            model.projectName = to.GetProjectName();
            model.companyName = to.GetCompanyName();
            model.companyShortName = to.GetCompanyShortName();
            model.repoLocation = to.GetRepoLocation();
            model.unityInstallLocation = to.GetUnityInstallLocation();
            return model;
        }

        private FileEntry GetSettingsFileInfo(bool isCreateIfNotPresent)
        {
            String filePath = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.CONFIG_FILE_ASSET_LOCATION);
            FileEntry entry = null;
            if(isCreateIfNotPresent){
                entry = fsController.GetExistingOrNewlyCreatedFile(filePath);
            } else {
                entry = fsController.GetExistingFile(filePath);
            }
            return entry;
        }

        private VSModuleSettingsTO GetDefaultTO()
        {
            return new VSModuleSettingsTO.Builder().Build();
        }

        

      
        
    }
}
