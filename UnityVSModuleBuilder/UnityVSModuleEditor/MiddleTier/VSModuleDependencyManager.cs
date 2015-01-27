using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    class VSModuleDependencyManager
    {
        private readonly UnityApi unityApi;
        private VSModuleXmlSerializer serializer;

        public VSModuleDependencyManager(UnityApi unityApi, VSModuleXmlSerializer serializer)
        {
            this.unityApi = unityApi;
            this.serializer = serializer;
        }

        public VSModuleDependencyTO GetDependencyTO()
        {
            VSModuleDependencyTO to = null;
            try
            {
                FileInfo dependencyFileInfo = GetDependencyFileInfo();
                if (dependencyFileInfo.Exists)
                {
                    to = GetToFromSerializer(dependencyFileInfo);
                }
                else
                {
                    unityApi.Log("Dependency Info Not Found For Module.");
                }
                if (to == null)
                {
                    unityApi.Log("Creating new default dependency info for failed load.");
                    to = new VSModuleDependencyTO(new List<VSModuleDependencyItem>());
                }
                
            }
            catch (Exception e)
            {
                unityApi.LogError("Unexpected Exception occurred getting dependency info for VSModule.", e);
            }
            return to;
        }

        private FileInfo GetDependencyFileInfo()
        {
            String dependencyFilePath = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.DEPENDENCY_FILE_LOCATION);
            return new FileInfo(dependencyFilePath);
        }

        private VSModuleDependencyTO GetToFromSerializer(FileInfo dependencyFileInfo)
        {
            VSModuleDependencyTO to = null;
            VSModuleDependencyXmlModel model = serializer.GetDeserializedDependency(dependencyFileInfo);
            if (model != null)
            {
                to = TranslateModelToTo(model);
            }
            else
            {
                unityApi.Log("Failed to Load Dependency Info From XML.");
            }
            return to;
        }

        private VSModuleDependencyTO TranslateModelToTo(VSModuleDependencyXmlModel model)
        {
            List<VSModuleDependencyItem> toDependencies = new List<VSModuleDependencyItem>();
            VSModuleDependencyTO to = new VSModuleDependencyTO(toDependencies);
            foreach (DependencyItem item in model.dependencies)
            {
                VSModuleDependencyItem toDependency = new VSModuleDependencyItem(item.companyShortName, item.projectName);
                toDependencies.Add(toDependency);
            }
            return to;
        }

        internal bool AddDependency(string companyShortName, string projectName)
        {
            bool isAdded = false;
            try
            {
                VSModuleDependencyTO to = GetDependencyTO();
                if (IsDependencyPresent(to, companyShortName, projectName))
                {
                    unityApi.LogError("Cannot Add Dependency. Dependency Already Present");
                }
                else
                {
                    AddVerifiedNewDependency(to, companyShortName, projectName);
                    isAdded = true;
                }
            }
            catch (Exception e)
            {
                unityApi.LogError("Unexpected Exception Adding Dependency. See Error Log For Details", e);
            }
            return isAdded;
        }

        private void AddVerifiedNewDependency(VSModuleDependencyTO to, string companyShortName, string projectName)
        {
            VSModuleDependencyXmlModel model = TranslateTOtoModel(to);
            DependencyItem item = new DependencyItem();
            item.companyShortName = companyShortName;
            item.projectName = projectName;
            model.dependencies.Add(item);
            FileInfo dependencyFileInfo = GetDependencyFileInfo();
            serializer.SerializeDependencyModel(dependencyFileInfo, model);
        }

        private VSModuleDependencyXmlModel TranslateTOtoModel(VSModuleDependencyTO to)
        {
            VSModuleDependencyXmlModel model = new VSModuleDependencyXmlModel();
            model.dependencies = new List<DependencyItem>();
            List<VSModuleDependencyItem>.Enumerator toDependencies = to.GetDependencies();
            while (toDependencies.MoveNext())
            {
                DependencyItem modelDependency = new DependencyItem();
                modelDependency.companyShortName = toDependencies.Current.GetCompanyShortName();
                modelDependency.projectName = toDependencies.Current.GetProjectName();
                model.dependencies.Add(modelDependency);
            }
            return model;
        }

        private bool IsDependencyPresent(VSModuleDependencyTO to, String companyShortName, String projectName)
        {
            bool isDependencyPresent = false;
            List<VSModuleDependencyItem>.Enumerator dependencies = to.GetDependencies();
            while (dependencies.MoveNext())
            {
                bool isCompanyEqual = String.Equals(companyShortName, dependencies.Current.GetCompanyShortName(), StringComparison.OrdinalIgnoreCase);
                bool isProjectEqual = String.Equals(projectName, dependencies.Current.GetProjectName(), StringComparison.OrdinalIgnoreCase);
                if (isCompanyEqual && isProjectEqual)
                {
                    isDependencyPresent = true;
                    break;
                }
            }
            return isDependencyPresent;
        }
    }
}
