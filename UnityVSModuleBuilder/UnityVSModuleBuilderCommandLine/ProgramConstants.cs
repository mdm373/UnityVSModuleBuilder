using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.CommandLine
{
    internal class ProgramConstants
    {
        public static string PROJECT_NAME_ARG = "P";
        public static string COMPANY_NAME_ARG = "CN";
        public static string COMPANY_SHORT_NAME_ARG = "CSN";
        public static string LOCATION_NAME_ARG = "L";
        public static string REPO_NAME_ARG = "R";
        public static string UNITY_ARG = "U";
        
        public const string ARG_IDENTIFIER = "/";
        public const string HELP_IDENTIFIER = "HELP";
        public const string HELP_FORMAT = @"
{0} {1}p projectName {1}cn companyName {1}csn companyShortName 
{1}l outputLocation {1}r repositoryLocation {1}u unityLocation

    projectName         The name Of the VSModule project. Used for repository 
                        identity, Unity project name, android identifier, 
                        Unity asset structure and VS project name
    companyName         Your company's full name. Used in Unity project player
                        settings.
    companyShortName    Short identifier for your company. Used in Unity asset
                        structure, repository identity and andriod identifier
    outputLocation      Destination directory for project generation
    repositoryLocation  Root directory location for managing project depenency 
                        modules
    unityLocation       Install location of Unity. Used for unity references in
                        VS projects
";
        
    }
}
