using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    public class VSModuleDelegate
    {
        private readonly VSModuleSettingsManager vsSettingsManager;
        private readonly VSModuleProjectManager vsProjectManager;
        private readonly UnityApi unityApi;
        private readonly VSModuleImportManager vsModuleImportExportManager;
        private VSModuleDependencyManager vsDependencyManager;

        public VSModuleDelegate(UnityApi unityApi, VSModuleXmlSerializer serializer)
        {
            this.vsSettingsManager = new VSModuleSettingsManager(unityApi, serializer);
            this.vsProjectManager = new VSModuleProjectManager(unityApi);
            this.vsDependencyManager = new VSModuleDependencyManager(unityApi, serializer);
            this.vsModuleImportExportManager = new VSModuleImportManager(unityApi);
            this.unityApi = unityApi;
        }

        public void SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            VSModuleSettingsTO origional = vsSettingsManager.RetrieveModuleSettingsTO();
            if(vsSettingsManager.SaveModuleSettingsTO(to)){
                unityApi.Log("VSModule Settings Saved.");
                
                vsProjectManager.UpdateVisualStudioProjects(origional.GetXmlModel(), to.GetXmlModel());
                unityApi.Log("Visual Studio Projects Updated For Unity Location Change.");
            }
        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            VSModuleSettingsTO to = vsSettingsManager.RetrieveModuleSettingsTO();
            if (to != null)
            {
                unityApi.Log("VSModule Settings Loaded.");
            }
            return to;
        }

        public void ExportModuleToRepository()
        {
            VSModuleSettingsTO to = RetrieveModuleSettingsTO();
            bool isExported = vsModuleImportExportManager.ExportModule(to);
            if (isExported)
            {
                unityApi.Log("VSModule Exported To Repository.");
            }

        }

        public VSModuleDependencyTO RetrieveModuleDependenciesTO()
        {
            VSModuleDependencyTO to = vsDependencyManager.GetDependencyTO();
            unityApi.Log("VSModule Dependencies Loaded.");
            return to;
        }

        public void AddModuleDependency(String companyShortName, String projectName)
        {
            VSModuleDependencyTO origional =  vsDependencyManager.GetDependencyTO();
            bool isAdded = vsDependencyManager.AddDependency(companyShortName, projectName);
            VSModuleSettingsTO settings = vsSettingsManager.RetrieveModuleSettingsTO();
            if (isAdded && settings != null)
            {
                VSModuleDependencyTO updated = vsDependencyManager.GetDependencyTO();
                isAdded = vsProjectManager.UpdateVisualStudioProjects(origional, updated, settings);
                if (isAdded)
                {
                    isAdded = vsModuleImportExportManager.ImportModule(companyShortName, projectName, settings);
                    if (isAdded)
                    {
                        unityApi.Log("Dependency '" + companyShortName + "' '" + projectName + "' added.");
                    }
                }
            }       
        }
    }
}
