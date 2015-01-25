using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace UnityVSModuleEditor.MSBuildProcess
{
    [XmlRootAttribute(Namespace = "http://schemas.microsoft.com/developer/msbuild/2003", IsNullable = false)]
    [SerializableAttribute()]
    public class Project
    {
        [XmlAttribute("ToolsVersion")]
        public String toolsVersion;

        [XmlAttribute("DefaultTargets")]
        public String DefaultTargets;

        [XmlElement("Import", Order=1)]
        public Import import;

        [XmlElement("PropertyGroup", Order=2)]
        public PropertyGroup propertyGroup1;

        [XmlElement("PropertyGroup", Order = 3)]
        public PropertyGroup propertyGroup2;

        [XmlElement("PropertyGroup", Order = 4)]
        public PropertyGroup propertyGroup3;

        [XmlElement("ItemGroup", Order = 5)]
        public ItemGroup itemGroup;

        [XmlElement("Import", Order = 6)]
        public Import import2;

        [XmlElement("PropertyGroup", Order = 7)]
        public PropertyGroup propertyGroup4;
        
    }

    public class ItemGroup {
        [XmlElement("Reference")]
        public List<Reference> references;
    }
    public class Import
    {
        [XmlAttribute("Project")]
        public String project;

        [XmlAttribute("Condition")]
        public String condition;
    }
    public class PropertyGroup
    {
        [XmlAttribute("Condition")]
        public String condition;

        [XmlElement("Configuration")]
        public Conditional configuration;

        [XmlElement("Platform")]
        public Conditional platform;

        [XmlElement("ProjectGuid")]
        public TextElement projectGuid;

        [XmlElement("OutputType")]
        public TextElement outputType;

        [XmlElement("AppDesignerFolder")]
        public TextElement appDesignerFolder;

        [XmlElement("RootNamespace")]
        public TextElement rootNamespace;

        [XmlElement("AssemblyName")]
        public TextElement assemblyName;

        [XmlElement("TargetFrameworkVersion")]
        public TextElement targetFrameworkVersion;

        [XmlElement("FileAlignment")]
        public TextElement fileAlignment;

        [XmlElement("TargetFrameworkProfile")]
        public TextElement targetFrameworkProfile;

        [XmlElement("UnityRoot")]
        public TextElement unityRoot;

        [XmlElement("PluginsRoot")]
        public TextElement pluginsRoot;

        [XmlElement("DebugSymbols")]
        public TextElement debugSymbols;

        [XmlElement("DebugType")]
        public TextElement debugType;

        [XmlElement("Optimize")]
        public TextElement optimize;

        [XmlElement("OutputPath")]
        public TextElement outputPath;

        [XmlElement("DefineConstants")]
        public TextElement defineConstants;

        [XmlElement("ErrorReport")]
        public TextElement errorReport;

        [XmlElement("WarningLevel")]
        public TextElement warningLevel;

        [XmlElement("PostBuildEvent")]
        public TextElement postBuildEvent;

        [XmlElement("PluginsEditorRoot")]
        public TextElement pluginsEditorRoot;
        
    }
    public class Conditional
    {
        [XmlAttribute("Condition")]
        public String condition;

        [XmlText]
        public String text;
    }
    public class TextElement
    {
        [XmlText]
        public String text;
    }
    public class Reference
    {
        [XmlAttribute("Include")]
        public String include;

        [XmlElement("HintPath")]
        public TextElement hintPath;

        [XmlElement("Private")]
        public TextElement privateElement;
    }
    public class Target
    {
        [XmlAttribute("Name")]
        public String name;
    }
}
