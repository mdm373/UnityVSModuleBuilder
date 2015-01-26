using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        [XmlAttribute("CompanyName")]
        public String companyName;
    }
}
