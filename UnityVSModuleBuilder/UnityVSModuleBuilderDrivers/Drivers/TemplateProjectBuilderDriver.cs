using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Drivers
{
    class TemplateProjectBuilderDriver : Drivable
    {
        public int Drive(string[] args)
        {
            String projectName = "DriverProject";
            String copyLocation = "GeneratedDriverProject";
            String companyName = "DriverCompany";

            TemplateProjectBuilder builder = TemplateProjectFactory.GetNewTemplateProjectBuilder();
            BuildProjectRequest request = TemplateProjectFactory.GetNewRequest(projectName, copyLocation, companyName);

            BuildProjectResponse response = builder.DoBuild(request);
            Logger.Log(response.ToString());
            return 0;
        }
    }
}
