using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon.Logging;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleDependencyManager
    {
        VSModuleDependencyTO GetDependencyTO();
        bool AddDependency(string companyShortName, string projectName);
    }
    
    internal class VSModuleDependencyManagerImpl : VSModuleDependencyManager
    {
        private readonly UnityApi unityApi;
        private XmlSerializerWrapper serializer;
        private FileSystemController fileSystemController;

        public VSModuleDependencyManagerImpl(UnityApi unityApi, XmlSerializerWrapper serializer, FileSystemController fsController)
        {
            this.unityApi = unityApi;
            this.serializer = serializer;
            this.fileSystemController = fsController;
        }

        public VSModuleDependencyTO GetDependencyTO()
        {
            VSModuleDependencyTO to = null;
            try
            {
                FileEntry dependencyFileInfo = GetDependencyFileInfo(false);
                if (dependencyFileInfo != null && dependencyFileInfo.IsPresent())
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

        private FileEntry GetDependencyFileInfo(bool isCreatedIfNotPresent)
        {
            String dependencyFilePath = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.DEPENDENCY_FILE_LOCATION);
            FileEntry entry = null;
            if (isCreatedIfNotPresent)
            {
                entry = this.fileSystemController.GetExistingFile(dependencyFilePath);
            } else {
                entry = this.fileSystemController.GetExistingOrNewlyCreatedFile(dependencyFilePath);
            }
            return entry;
        }

        private VSModuleDependencyTO GetToFromSerializer(FileEntry dependencyFileInfo)
        {
            VSModuleDependencyTO to = null;
            VSModuleDependencyXmlModel model = serializer.GetDeserialized<VSModuleDependencyXmlModel>(dependencyFileInfo);
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

        public bool AddDependency(string companyShortName, string projectName)
        {
            bool isAdded = false;
            try
            {
                VSModuleDependencyTO to = GetDependencyTO();
                if (IsDependencyPresent(to, companyShortName, projectName))
                {
                    Logger.LogError("Cannot Add Dependency. Dependency Already Present");
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
            FileEntry dependencyFileInfo = GetDependencyFileInfo(true);
            serializer.SerializeToFile <VSModuleDependencyXmlModel>(dependencyFileInfo, model);
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
