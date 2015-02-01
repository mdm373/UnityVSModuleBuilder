using System;
using System.Collections.Generic;

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

        internal bool ContainsMatchingItem(VSModuleDependencyItem item)
        {
            Boolean containsItem = false;
            foreach (VSModuleDependencyItem existingItem in dependencies)
            {
                if (item.Equals(existingItem))
                {
                    containsItem = true;
                    break;
                }
            }
            return containsItem;
        }
    }

    public class VSModuleDependencyItem : IEquatable<VSModuleDependencyItem>
    {
        private readonly string companyShortName;
        private readonly string projectName;

        public VSModuleDependencyItem(String companyShortName, String projectName) 
        {
            this.companyShortName = companyShortName.Trim();
            this.projectName = projectName.Trim();
        }

        public String GetCompanyShortName()
        {
            return this.companyShortName;
        }
        public String GetProjectName()
        {
            return this.projectName;
        }

        public bool Equals(VSModuleDependencyItem other)
        {
            Boolean isEqual = companyShortName.Equals(other.companyShortName, StringComparison.CurrentCultureIgnoreCase);
            isEqual = isEqual && projectName.Equals(other.projectName, StringComparison.CurrentCultureIgnoreCase);
            return isEqual;
        }

        public string GetDisplayValue()
        {
            return this.companyShortName + ":" + this.projectName;
        }
    }
}
