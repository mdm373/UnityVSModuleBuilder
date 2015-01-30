using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon.Logging;

namespace UnityVSModuleEditor.XMLStore
{
    internal interface XmlSerializerWrapper
    {
        bool SerializeToFile<V>(FileEntry info, V model);
        V GetDeserialized<V>(FileEntry file);
    }

    internal class XmlSerializerWrapperImpl : XmlSerializerWrapper
    {

        public bool SerializeToFile<V>(FileEntry info, V model)
        {
            bool isSuccess = false;
            FileStream fileStream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(V));
                fileStream = new FileStream(info.GetFilePath(), FileMode.Create);
                serializer.Serialize(fileStream, model);
                isSuccess = true;
            }
            catch (Exception e)
            {
                isSuccess = false;
                Logger.LogError("Exception serializing VSModule Settings. See Log for exception details",e );
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream);
            }
            return isSuccess;
        }

        public V GetDeserialized<V>(FileEntry file){
            V model = default(V);
            FileStream fileStream = null;
            try{
                XmlSerializer serializer = new XmlSerializer(typeof(V));
                fileStream = new FileStream(file.GetFilePath(), FileMode.Open);
                model = (V)serializer.Deserialize(fileStream);
            }
            catch (Exception e)
            {
                String fileName = (file == null) ? "NULL FILE" : file.GetFileName();
                String typeName = typeof(V).Name;
                Logger.LogError("Unexpected Exception deserializing File '" + fileName + "' to Type '" + typeName + "'. "
                        + "See Logged Error For Details", e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream);
            }
            
            return model;
        }
        
    }
}
