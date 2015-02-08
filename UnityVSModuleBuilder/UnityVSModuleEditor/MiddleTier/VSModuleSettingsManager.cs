using System;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;
using UnityVSModuleEditor.XMLStore;
using UnityVSModuleCommon.Application;

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
        private ApplicationManager appManager;

        public VSModuleSettingsManagerImpl(UnityApi unityApi,
            XmlSerializerWrapper serializer, 
            FileSystemController fsController,
            ApplicationManager appManager)
        {
            this.unityApi = unityApi;
            this.serializer = serializer;
            this.fsController = fsController;
            this.appManager = appManager;
        }

        public bool SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            bool isSaved = true;
            try
            {
                SendToToFile(to);
                ApplicationSettings oldSettings = GetExistingAppSettings();
                if(!oldSettings.GetRepoLocation().Equals(to.GetRepoLocation()))
                {
                    ApplicationSettings settings = ApplicationFactory.GetNewApplicationSettings(to.GetRepoLocation());
                    appManager.SaveApplicationSettings(settings);
                }
            }
            catch (Exception e)
            {
                isSaved = false;
                Logger.LogError("Could Not Save VSMoule Settings. Unexpected Error. See Exception for Details.", e);
            }
            return isSaved;
        }

        private ApplicationSettings GetExistingAppSettings()
        {
            ApplicationSettings settings = null;
            ApplicationSettingsResponse response = appManager.RetrieveApplicationSettings();
            if (response.GetCode() == AppSettingsCode.SUCCESS)
            {
                settings = response.GetApplicationSettings();
            }
            return settings;
        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            VSModuleSettingsTO to = null;
            try
            {
                ApplicationSettings appSettings = GetExistingAppSettings();
                if (appSettings != null)
                {
                    to = GetTOFromFile(appSettings);
                }
                else
                {
                    to = GetDefaultTO();
                }
                
            }
            catch (Exception e)
            {
                Logger.LogError("Could Not Populate VSModule Settings. Unexpected Error. See Exception for Details.", e);
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

        private VSModuleSettingsTO GetTOFromFile(ApplicationSettings appSettings)
        {
            VSModuleSettingsTO to = null;
            FileEntry info = GetSettingsFileInfo(false);
            if (info != null && info.IsPresent())
            {
                VSModuleSettingsXmlModel model = serializer.GetDeserialized<VSModuleSettingsXmlModel>(info);
                if (model != null)
                {
                    to = TranslateModeltoTO(model, appSettings);
                }
            }
            else
            {
                Logger.LogError("Could not initialize VSModule settings. Config File Not Found From Asset Root. '" + VSModuleConstants.CONFIG_FILE_ASSET_LOCATION + '\'');
            }
            return to;
        }

        private VSModuleSettingsTO TranslateModeltoTO(VSModuleSettingsXmlModel model, ApplicationSettings settings)
        {
            VSModuleSettingsTO.Builder builder = new VSModuleSettingsTO.Builder();
            builder.ProjectName = model.projectName;
            builder.CompanyName = model.companyName;
            builder.CompanyShortName = model.companyShortName;
            builder.UnityInstallLocation = model.unityInstallLocation;
            builder.RepoLocation = settings.GetRepoLocation();
            return builder.Build();
        }

        private VSModuleSettingsXmlModel TranslateTOtoModel(VSModuleSettingsTO to)
        {
            VSModuleSettingsXmlModel model = new VSModuleSettingsXmlModel();
            model.projectName = to.GetProjectName();
            model.companyName = to.GetCompanyName();
            model.companyShortName = to.GetCompanyShortName();
            model.unityInstallLocation = to.GetUnityInstallLocation();
            return model;
        }

        private FileEntry GetSettingsFileInfo(bool isCreateIfNotPresent)
        {
            String filePath = Path.Combine(unityApi.GetProjectFolder(), VSModuleConstants.CONFIG_FILE_ASSET_LOCATION);
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
            VSModuleSettingsTO.Builder builder = new VSModuleSettingsTO.Builder();
            builder.CompanyName = String.Empty;
            builder.CompanyShortName = String.Empty;
            builder.ProjectName = String.Empty;
            builder.UnityInstallLocation = String.Empty;
            builder.RepoLocation = String.Empty;
            return builder.Build();
        }

        

      
        
    }
}
