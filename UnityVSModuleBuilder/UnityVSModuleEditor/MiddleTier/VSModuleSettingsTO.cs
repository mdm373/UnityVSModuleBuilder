
namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleSettingsTO
    {
        private readonly string projectName;
        private readonly string companyName;
        private readonly string companyShortName;
        private string repoLocation;
        private string unityInstallLocation;

        internal class Builder
        {
            public string ProjectName { get; set; }
            public string CompanyName { get; set; }
            public string CompanyShortName { get; set; }
            public string RepoLocation { get; set; }
            public string UnityInstallLocation { get; set; }
            public VSModuleSettingsTO Build()
            {
                return new VSModuleSettingsTO(this);
            }
        }
        
        private VSModuleSettingsTO(Builder builder)
        {
            this.projectName = builder.ProjectName;
            this.companyName = builder.CompanyName;
            this.companyShortName = builder.CompanyShortName;
            this.repoLocation = builder.RepoLocation;
            this.unityInstallLocation = builder.UnityInstallLocation;
        }

        public string GetProjectName()
        {
            return this.projectName;
        }

        public string GetCompanyName()
        {
            return this.companyName;
        }

        public string GetCompanyShortName()
        {
            return this.companyShortName;
        }

        public string GetRepoLocation()
        {
            return this.repoLocation;
        }

        public void SetRepoLocation(string repoLocation)
        {
            this.repoLocation = repoLocation;
        }

        public string GetUnityInstallLocation()
        {
            return this.unityInstallLocation;
        }

        public void SetUnityInstallLocation(string unityInstallLocation)
        {
            this.unityInstallLocation = unityInstallLocation;
        }

    }
}
