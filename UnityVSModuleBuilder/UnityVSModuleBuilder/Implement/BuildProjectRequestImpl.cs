using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Implement
{
    public class BuildProjectRequestImpl : BuildProjectRequest
    {
        private string copyLocation;
        private string projectName;

        public void SetProjectName(String projectName)
        {
            this.projectName = projectName;
        }

        public string GetProjectName()
        {
            return projectName;
        }

        public void SetCopyLocation(String copyLocation)
        {
            this.copyLocation = copyLocation;
        }

        public string GetCopyLocation()
        {
            return copyLocation;
        }
    }
}
