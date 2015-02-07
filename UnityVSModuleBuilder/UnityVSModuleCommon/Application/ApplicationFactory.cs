using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleCommon.Application
{
    public sealed class ApplicationFactory
    {
        private ApplicationFactory() { }
        public static ApplicationManager GetNewApplicationManager()
        {
            FileSystemController fsController = FileSystemFactory.GetNewFileSystemController();
            XmlSerializerWrapper serializer = XmlSerializerFactory.GetXmlSerializerWrapper();
            RegistryController registryController = RegistryFactory.GetRegistryController();
            return new ApplicationManagerImpl(registryController, fsController, serializer);
        }

        public static ApplicationSettings GetNewApplicationSettings(String repoLocation)
        {
            return new ApplicationSettingsImpl(repoLocation);
        }
    }
}
