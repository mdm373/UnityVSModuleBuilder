using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UnityVSModuleEditor.XMLStore
{
    [XmlRootAttribute("VSModuleDependency")]
    public class VSModuleDependencyXmlModel
    {
        [XmlElement("DepenceyEntry")]
        public List<DependencyItem> dependencies;
    }

    public class DependencyItem
    {
        [XmlAttribute("ProjectName")]
        public String projectName;

        [XmlAttribute("CompanyShortName")]
        public String companyShortName;
    }
}
