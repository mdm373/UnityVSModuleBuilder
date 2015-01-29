using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleDependencyTO
    {
        private readonly List<VSModuleDependencyItem> dependencies;

        public VSModuleDependencyTO(List<VSModuleDependencyItem> dependencies)
        {
            this.dependencies = dependencies;
        }

        public List<VSModuleDependencyItem>.Enumerator GetDependencies()
        {
            return dependencies.GetEnumerator();
        }

        public int GetDependencyCount()
        {
            return dependencies.Count;
        }
    }

    public class VSModuleDependencyItem
    {
        private readonly string companyShortName;
        private readonly string projectName;

        public VSModuleDependencyItem(String companyShortName, String projectName) 
        {
            this.companyShortName = companyShortName;
            this.projectName = projectName;
        }

        public String GetCompanyShortName()
        {
            return this.companyShortName;
        }
        public String GetProjectName()
        {
            return this.projectName;
        }
    }
}
