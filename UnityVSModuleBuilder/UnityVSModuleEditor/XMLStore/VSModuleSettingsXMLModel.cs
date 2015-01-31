using System;
using System.Xml.Serialization;

namespace UnityVSModuleEditor.XMLStore
{
    [XmlRootAttribute("ModuleConfig")]
    public class VSModuleSettingsXmlModel
    {
        [XmlAttribute("projectName")]
        public string projectName = String.Empty;

        [XmlAttribute("companyName")]
        public string companyName = String.Empty;

        [XmlAttribute("companyShortName")]
        public string companyShortName = String.Empty;

        [XmlAttribute("moduleRepositoryLocation")]
        public string repoLocation = String.Empty;

        [XmlAttribute("unityLocation")]
        public string unityInstallLocation = String.Empty;
    }
}
