using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.MSBuildProcess;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    class VSModuleSettingsManager
    {
        private const string CONFIG_FILE_LOCATION = @"Editor\ModuleConfig.xml";
        private readonly UnityApi unityApi;

        public VSModuleSettingsManager(UnityApi unityApi)
        {
            this.unityApi = unityApi;
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
                unityApi.Log("VSModule Settings Loaded from '" + CONFIG_FILE_LOCATION + '\'');
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
                unityApi.LogWarning("Config File Not Found On Save. Creating new VSModule Config file at '" + CONFIG_FILE_LOCATION + '\'');
                File.Create(info.FullName);
            }
            SerializeTO(info, to);
        }

        private VSModuleSettingsTO GetTOFromFile()
        {
            VSModuleSettingsTO to = null;
            FileInfo info = GetSettingsFileInfo();
            if (info.Exists)
            {
                to = GetDeserializedTO(info);
            }
            else
            {
                unityApi.LogWarning("Could not initialize VSModule settings. Config File Not Found From Asset Root. '" + CONFIG_FILE_LOCATION + '\'');
            }
            return to;
        }

        private FileInfo GetSettingsFileInfo()
        {
            String filePath = Path.Combine(unityApi.GetAssetFolder(), CONFIG_FILE_LOCATION);
            return new FileInfo(filePath);
        }

        private VSModuleSettingsTO GetDefaultTO()
        {
            VSModuleSettingsXMLModel model = new VSModuleSettingsXMLModel();
            return new VSModuleSettingsTO(model);
        }

        private VSModuleSettingsTO GetDeserializedTO(FileInfo info)
        {
            FileStream fileStream = null;
            VSModuleSettingsTO to = null;
            try
            {
                XmlSerializer serializer = GetVSModuleSettingsXmlModelSerializer();
                fileStream = new FileStream(info.FullName, FileMode.Open);
                VSModuleSettingsXMLModel deserialized = (VSModuleSettingsXMLModel)serializer.Deserialize(fileStream);
                to = new VSModuleSettingsTO(deserialized);
            }
            catch (Exception e)
            {
                unityApi.LogError("Exception deserializing VSModule Settings. See Log for exception details");
                unityApi.LogException(e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream, unityApi);
            }


            return to;

        }

        private void SerializeTO(FileInfo info, VSModuleSettingsTO to)
        {
            FileStream fileStream = null;
            try
            {
                XmlSerializer serializer = GetVSModuleSettingsXmlModelSerializer();
                fileStream = new FileStream(info.FullName, FileMode.Create);
                serializer.Serialize(fileStream, to.GetXmlModel());
            }
            catch (Exception e)
            {
                unityApi.LogError("Exception serializing VSModule Settings. See Log for exception details");
                unityApi.LogException(e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream, unityApi);
            }
            
            
        }

        private XmlSerializer GetVSModuleSettingsXmlModelSerializer()
        {
            return new XmlSerializer(typeof(VSModuleSettingsXMLModel));
        }
    }
}
