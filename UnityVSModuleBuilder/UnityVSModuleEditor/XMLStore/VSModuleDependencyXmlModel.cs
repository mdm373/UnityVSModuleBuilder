﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnityVSModuleEditor.XMLStore
{
    [XmlRootAttribute("VSModuleDependency")]
    internal class VSModuleDependencyXmlModel
    {
        [XmlElement("DepenceyEntry")]
        public List<DependencyItem> dependencies;
    }

    internal class DependencyItem
    {
        [XmlAttribute("ProjectName")]
        public String projectName;

        [XmlAttribute("CompanyShortName")]
        public String companyShortName;
    }
}
