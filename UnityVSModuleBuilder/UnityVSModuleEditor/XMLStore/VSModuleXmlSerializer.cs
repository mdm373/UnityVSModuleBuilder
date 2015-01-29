using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor.XMLStore
{
    internal class VSModuleXmlSerializer
    {
        private readonly UnityApi unityApi;

        public VSModuleXmlSerializer(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void SerializeSettingsModel(FileInfo info, VSModuleSettingsXmlModel model)
        {
            FileStream fileStream = null;
            try
            {
                XmlSerializer serializer = GetVSModuleSettingsXmlModelSerializer();
                fileStream = new FileStream(info.FullName, FileMode.Create);
                serializer.Serialize(fileStream, model);
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

        public VSModuleSettingsXmlModel GetDeserializedSettings(FileInfo info)
        {
            FileStream fileStream = null;
            VSModuleSettingsXmlModel model = null;
            try
            {
                XmlSerializer serializer = GetVSModuleSettingsXmlModelSerializer();
                fileStream = new FileStream(info.FullName, FileMode.Open);
                model = (VSModuleSettingsXmlModel)serializer.Deserialize(fileStream);
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


            return model;

        }

        private XmlSerializer GetVSModuleSettingsXmlModelSerializer()
        {
            return new XmlSerializer(typeof(VSModuleSettingsXmlModel));
        }

        public VSModuleDependencyXmlModel GetDeserializedDependency(FileInfo dependencyFileInfo)
        {
            FileStream fileStream = null;
            VSModuleDependencyXmlModel model = null;
            try
            {
                XmlSerializer serializer = GetVSModuleDependencyXmlModelSerializer();
                fileStream = new FileStream(dependencyFileInfo.FullName, FileMode.Open);
                model = (VSModuleDependencyXmlModel)serializer.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                unityApi.LogError("Exception deserializing VSModule Dependency XML. See Log for exception details", e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream, unityApi);
            }


            return model;
        }

        private XmlSerializer GetVSModuleDependencyXmlModelSerializer()
        {
            return new XmlSerializer(typeof(VSModuleDependencyXmlModel));
        }

        internal void SerializeDependencyModel(FileInfo info, VSModuleDependencyXmlModel model)
        {
            FileStream fileStream = null;
            try
            {
                XmlSerializer serializer = GetVSModuleDependencyXmlModelSerializer();
                fileStream = new FileStream(info.FullName, FileMode.Create);
                serializer.Serialize(fileStream, model);
            }
            catch (Exception e)
            {
                unityApi.LogError("Exception Serializing VSModule Dependencies. See Log for Exception Details");
                unityApi.LogException(e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream, unityApi);
            }
        }
    }
}
