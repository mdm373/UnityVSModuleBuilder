using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor.XMLStore
{
    public class VSModuleXmlSerializer
    {
        private readonly UnityApi unityApi;

        public VSModuleXmlSerializer(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void SerializeModel(FileInfo info, VSModuleSettingsXmlModel model)
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

        public VSModuleSettingsXmlModel GetDeserializedModel(FileInfo info)
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
    }
}
