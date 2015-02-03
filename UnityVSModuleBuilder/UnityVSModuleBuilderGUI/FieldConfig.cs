using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;
using Microsoft.Win32;
using System.IO;

namespace UnityVSModuleBuilder.GUI
{
    internal class FieldConfig : BuildProjectRequest
    {
        private const string DEFAULT_GENERATION_LOCATION = "GeneratedProject";
        private const string CONFIG_FILE_LOCATION = ".uiState.xml";
        private const string UNITY_INSTALL_REG_KEY = @"HKEY_CURRENT_USER\Software\Unity Technologies\Installer\Unity 5.0.0b9";
        private const string UNITY_INSTALL_REG_VALUE = "Location x64";
        
        private FieldConfigXml xml;
        
        public FieldConfig(FieldConfigXml xml)
        {
            this.xml = xml;
        }

        public String ProjectName { get { return xml.projectName; } set { xml.projectName = value; } }
        public String CompanyName { get { return xml.companyName; } set { xml.companyName = value; } }
        public String CompanyShortName { get { return xml.companyShortName; } set { xml.companyShortName = value; } }
        public String UnityInstallLocation { get { return xml.unityInstallLocation; } set { xml.unityInstallLocation = value; } }
        public String RepositoryLocation { get { return xml.respositoryLocation; } set { xml.respositoryLocation = value; } }
        public String ProjectGenerationLocation { get { return xml.generationLocation; } set { xml.generationLocation = value; } }

        public void SaveToFile()
        {
            FileSystemController fsController = FileSystemFactory.GetNewFileSystemController();
            FileEntry file = fsController.GetExistingOrNewlyCreatedFile(CONFIG_FILE_LOCATION);
            if (file != null && file.IsPresent())
            {
                XmlSerializerWrapper serializer = new XmlSerializerWrapperImpl();
                serializer.SerializeToFile<FieldConfigXml>(file, xml);
            }
        }

        public void Reset()
        {
            this.xml = new FieldConfigXml();
            PopulateDefaults(xml);
        }

        public static FieldConfig ReadFromFile()
        {
            FileSystemController fsController = FileSystemFactory.GetNewFileSystemController();
            FileEntry file = fsController.GetExistingFile(CONFIG_FILE_LOCATION);
            FieldConfigXml xml = null;
            if(file != null && file.IsPresent())
            {
                XmlSerializerWrapper serializer = new XmlSerializerWrapperImpl();
                xml = serializer.GetDeserialized<FieldConfigXml>(file);
            } else {
                xml = new FieldConfigXml();
                PopulateDefaults(xml);
            }
            return new FieldConfig(xml);
        }

        private static void PopulateDefaults(FieldConfigXml xml)
        {
            if (xml.unityInstallLocation == null)
            {
                xml.unityInstallLocation = GetUnityInstallLocationFromReg();
            }
            if (xml.generationLocation == null)
            {
                xml.generationLocation = DEFAULT_GENERATION_LOCATION;
            }
            if (xml.respositoryLocation == null)
            {
                xml.respositoryLocation = GetDefaultRepositoryLocation();
            }
        }

        private static string GetDefaultRepositoryLocation()
        {
            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                path = Directory.GetParent(path).FullName;
            }
            return System.IO.Path.Combine(path, "UVSRepo");
        }

        private static string GetUnityInstallLocationFromReg()
        {
            String unityInstall = String.Empty;
            try
            {
                unityInstall = (String)Registry.GetValue(UNITY_INSTALL_REG_KEY, UNITY_INSTALL_REG_VALUE, String.Empty);
            }
            catch (Exception e)
            {
                Console.Error.Write(e.ToString());
            }
            return unityInstall;
        }

        public string GetProjectName()
        {
            return ProjectName.Trim();
        }

        public string GetCopyLocation()
        {
            return ProjectGenerationLocation.Trim();
        }

        public string GetCompanyName()
        {
            return CompanyName.Trim();
        }

        public string GetCompanyShortName()
        {
            return CompanyShortName.Trim();
        }

        public string GetUnityLocation()
        {
            return UnityInstallLocation.Trim();
        }

        public string GetModuleRepositoryLocation()
        {
            return RepositoryLocation.Trim();
        }
    }

    [XmlRoot]
    public class FieldConfigXml
    {
        [XmlAttribute]
        public String projectName = String.Empty;
        [XmlAttribute]
        public String companyName = String.Empty;
        [XmlAttribute]
        public String companyShortName = String.Empty;
        [XmlAttribute]
        public String respositoryLocation = null;
        [XmlAttribute]
        public String unityInstallLocation = null;
        [XmlAttribute]
        public String generationLocation = null;
    }
}
