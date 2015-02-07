﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnityVSModuleCommon.XMLStore
{
    [XmlRootAttribute("ApplicationSettingsXMLModel")]
    public class ApplicationSettingsXMLModel
    {
        [XmlAttribute("repositoryLocation")]
        public string repoLocation;

    }
}
