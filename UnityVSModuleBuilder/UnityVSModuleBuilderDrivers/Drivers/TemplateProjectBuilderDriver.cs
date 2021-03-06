﻿using UnityVSModuleBuilder.Implement;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.Drivers
{
    internal class TemplateProjectBuilderDriver : Drivable
    {
        public int Drive(string[] args)
        {
            BuildProjectRequestImpl.Builder requestBuilder = new BuildProjectRequestImpl.Builder();
            requestBuilder.projectName = "DriverProject3";
            requestBuilder.copyLocation = "GeneratedDriverProject";
            requestBuilder.companyName = "DriverCompany";
            requestBuilder.companyShortName = "cn";
            requestBuilder.unityLocation = @"C:\Program Files\Unity 5.0.0b9\";
            
            BuildProjectRequest request = requestBuilder.Build();
            TemplateProjectBuilder builder = TemplateProjectFactory.GetNewTemplateProjectBuilder();
            BuildProjectResponse response = builder.DoBuild(request);
            
            Logger.Log(response.ToString());
            return 0;
        }
    }
}
