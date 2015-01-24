﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder
{

    public class BuildProjectRequestImpl : BuildProjectRequest
    {

        private readonly string copyLocation;
        private readonly string projectName;
        private readonly string companyName;
        private readonly string companyShortName;
        private readonly string unityLocation;
        private readonly string moduleRepositoryLocation;
        
        public class Builder{
            public string copyLocation;
            public string projectName;
            public string companyName;
            public string companyShortName;
            public string unityLocation;
            public string moduleRepositoryLocation;

            public BuildProjectRequest Build()
            {
                if (copyLocation == null) throw new ArgumentException("copyLocation cannot be null.");
                if (projectName == null) throw new ArgumentException("projectName cannot be null.");
                if (companyName == null) throw new ArgumentException("companyName cannot be null.");
                if (companyShortName == null) throw new ArgumentException("companyShortName cannot be null.");
                if (unityLocation == null) throw new ArgumentException("unityLocation cannot be null.");
                if (moduleRepositoryLocation == null) throw new ArgumentException("moduleRepoLocation cannot be null.");
                return new BuildProjectRequestImpl(this);
            }
        }

        public BuildProjectRequestImpl(Builder builder)
        {
            this.copyLocation = builder.copyLocation;
            this.projectName = builder.projectName;
            this.companyName = builder.companyName;
            this.companyShortName = builder.companyShortName;
            this.unityLocation = builder.unityLocation;
            this.moduleRepositoryLocation = builder.moduleRepositoryLocation;
        }
        
        public string GetProjectName()
        {
            return projectName;
        }

        public string GetCopyLocation()
        {
            return copyLocation;
        }


        public string GetCompanyName()
        {
            return companyName;
        }

        public string GetCompanyShortName()
        {
            return companyShortName;
        }

        public string GetUnityLocation()
        {
            return this.unityLocation;
        }

        public string GetModuleRepositoryLocation()
        {
            return this.moduleRepositoryLocation;
        }
    }
}
