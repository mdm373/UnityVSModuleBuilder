using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.XMLStore;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleCommon.Application
{
    internal class ApplicationManagerImpl : ApplicationManager
    {
        private const string REG_LOCATION = @"Software\UnityVSmoduleBuilder";
        private const string INSTALL_LOC_NAME = "install-location";
        private const string APP_SETTING_FILE = "AppSettings.xml";
        private const string REPO_FOLDER = "UVSRepo";

        private readonly RegistryController registryController;
        private readonly FileSystemController fsController;
        private readonly XmlSerializerWrapper serializer;
        
        
        public ApplicationManagerImpl(RegistryController registryController, 
            FileSystemController fsController, 
            XmlSerializerWrapper serializer)
        {
            this.registryController = registryController;
            this.fsController = fsController;
            this.serializer = serializer;
        }

        public ApplicationSettingsResponse RetrieveApplicationSettings()
        {
            ApplicationSettingsResponse response = null;
            try
            {
                String appSettingsFilePath = GetAppSettingsPath();
                if (appSettingsFilePath != null)
                {
                    response = GetResponseForSettingsFilePath(appSettingsFilePath);
                }
                else
                {
                    response = GetUnknownInstallPathResponse();
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Unknown Exception retrieving application settings. See log for details.", e);
                response = GetUnknownErrorResponse();
            }
            return response;
            
        }

        private ApplicationSettingsResponse GetUnknownErrorResponse()
        {
            return new ApplicationSettingsResponseImpl(null, AppSettingsCode.UNKNOWN_ERROR);
        }

        private ApplicationSettingsResponse GetResponseForSettingsFilePath(string appSettingsFilePath)
        {
            ApplicationSettingsResponse response = null;
            FileEntry appSettingsFile = fsController.GetExistingFile(appSettingsFilePath);
            if (appSettingsFile != null)
            {
                response = GetResponseForSettingsFilePath(appSettingsFile);
                
            }
            else
            {
                response = GetDefaultResponse();
            }
            return response;
        }

        private ApplicationSettingsResponse GetResponseForSettingsFilePath(FileEntry appSettingsFile)
        {
            ApplicationSettingsResponse response = null;
            ApplicationSettingsXMLModel model = serializer.GetDeserialized<ApplicationSettingsXMLModel>(appSettingsFile);
            if (model != null)
            {
                response = GetTranslatedModelResponse(model);
            }
            else
            {
                response = GetDefaultResponse();
            }
            return response;
        }

        private ApplicationSettingsResponse GetDefaultResponse()
        {
            String defaultRepoLocation = Path.Combine(GetInstallLocation(), REPO_FOLDER);
            ApplicationSettings settings = new ApplicationSettingsImpl(defaultRepoLocation);
            return new ApplicationSettingsResponseImpl(settings, AppSettingsCode.SUCCESS);
        }

        private ApplicationSettingsResponse GetUnknownInstallPathResponse()
        {
            return new ApplicationSettingsResponseImpl(null, AppSettingsCode.INSTALL_NOT_FOUND);
        }

        private string GetAppSettingsPath()
        {
            String settingsPath = null;
            String installLocation = GetInstallLocation();
            if (installLocation != null)
            {
                settingsPath = Path.Combine(installLocation, APP_SETTING_FILE);
            }
            else
            {
                Logger.LogError("Failed To Find Application Install Location Reg Key. Cannot Locate Settings File.");
            }
            return settingsPath;
        }

        private string GetInstallLocation()
        {
            return registryController.GetRegistryKey<String>(RegKeyType.HK_LOCAL_MACHINE, REG_LOCATION, INSTALL_LOC_NAME);
        }

        private ApplicationSettingsResponse GetTranslatedModelResponse(ApplicationSettingsXMLModel model)
        {
            String repoLocation = model.repoLocation;
            ApplicationSettings settings = new ApplicationSettingsImpl(repoLocation);
            ApplicationSettingsResponse response = new ApplicationSettingsResponseImpl(settings, AppSettingsCode.SUCCESS);
            return response;
        }


        public void SaveApplicationSettings(ApplicationSettings settings)
        {
            String appSettingsFilePath = GetAppSettingsPath();
            FileEntry appSettingsFile = fsController.GetExistingOrNewlyCreatedFile(appSettingsFilePath);
            ApplicationSettingsXMLModel model = GetTranslatedSettings(settings);
            serializer.SerializeToFile<ApplicationSettingsXMLModel>(appSettingsFile, model);
        }

        private ApplicationSettingsXMLModel GetTranslatedSettings(ApplicationSettings settings)
        {
            ApplicationSettingsXMLModel model = new ApplicationSettingsXMLModel();
            model.repoLocation = settings.GetRepoLocation();
            return model;
        }
    }
}
