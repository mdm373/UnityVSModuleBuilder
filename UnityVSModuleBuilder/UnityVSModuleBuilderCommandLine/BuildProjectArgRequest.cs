using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.CommandLine
{
    class BuildProjectArgRequest : BuildProjectRequest
    {
        private readonly ArgHelper argHelper;

        public BuildProjectArgRequest(ArgHelper argHelper)
        {
            this.argHelper = argHelper;
        }

        public string GetProjectName()
        {
            return argHelper.GetArgValue(ProgramConstants.PROJECT_NAME_ARG);
        }

        public string GetCopyLocation()
        {
            return argHelper.GetArgValue(ProgramConstants.LOCATION_NAME_ARG);
        }

        public string GetCompanyName()
        {
            return argHelper.GetArgValue(ProgramConstants.COMPANY_NAME_ARG);
        }

        public string GetCompanyShortName()
        {
            return argHelper.GetArgValue(ProgramConstants.COMPANY_SHORT_NAME_ARG);
        }

        public string GetUnityLocation()
        {
            return argHelper.GetArgValue(ProgramConstants.UNITY_ARG);
        }

        public string GetModuleRepositoryLocation()
        {
            return argHelper.GetArgValue(ProgramConstants.REPO_NAME_ARG);
        }
    }
}
