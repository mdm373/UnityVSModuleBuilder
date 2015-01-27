using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    class VSModuleSettingsManager
    {
        private readonly UnityApi unityApi;
        private readonly VSModuleXmlSerializer serializer;

        public VSModuleSettingsManager(UnityApi unityApi, VSModuleXmlSerializer serializer)
        {
            this.unityApi = unityApi;
            this.serializer = serializer;
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
                unityApi.LogError("Could Not Populate VSModule Settings. Unexpected Error. See Exception for Details.");
                unityApi.LogException(e);
                to = GetDefaultTO();
            }
            finally
            {
                if (to == null)
                {
                    unityApi.LogWarning("Using Default VSModule Settings. See log for error details loading settings from file.");
                    to = GetDefaultTO();
                }
            }
            return to;
        }

        

        private void SendToToFile(VSModuleSettingsTO to)
        {
            FileInfo info = GetSettingsFileInfo();
            if (!info.Exists)
            {
                unityApi.LogWarning("Config File Not Found On Save. Creating new VSModule Config file at '" + VSModuleConstants.CONFIG_FILE_ASSET_LOCATION + '\'');
                File.Create(info.FullName);
            }
            serializer.SerializeSettingsModel(info, to.GetXmlModel());
        }

        private VSModuleSettingsTO GetTOFromFile()
        {
            VSModuleSettingsTO to = null;
            FileInfo info = GetSettingsFileInfo();
            if (info.Exists)
            {
                VSModuleSettingsXmlModel model = serializer.GetDeserializedSettings(info);
                to = new VSModuleSettingsTO(model);
            }
            else
            {
                unityApi.LogWarning("Could not initialize VSModule settings. Config File Not Found From Asset Root. '" + VSModuleConstants.CONFIG_FILE_ASSET_LOCATION + '\'');
            }
            return to;
        }

        private FileInfo GetSettingsFileInfo()
        {
            String filePath = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.CONFIG_FILE_ASSET_LOCATION);
            return new FileInfo(filePath);
        }

        private VSModuleSettingsTO GetDefaultTO()
        {
            VSModuleSettingsXmlModel model = new VSModuleSettingsXmlModel();
            return new VSModuleSettingsTO(model);
        }

        

      
        
    }
}
