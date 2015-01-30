using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon.Logging;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleDelegate
    {
        void SaveModuleSettingsTO(VSModuleSettingsTO to);
        void ExportModuleToRepository();
        VSModuleSettingsTO RetrieveModuleSettingsTO();
        VSModuleDependencyTO RetrieveModuleDependenciesTO();
        void AddModuleDependency(String companyShortName, String projectName);
        
    }

    internal class VSModuleDelegateImpl : VSModuleDelegate
    {
        private readonly VSModuleSettingsManager vsSettingsManager;
        private readonly VSModuleProjectManager vsProjectManager;
        private readonly VSModuleImportExportManager vsModuleImportExportManager;
        private readonly VSModuleDependencyManager vsDependencyManager;

        public VSModuleDelegateImpl(VSModuleSettingsManager vsSettingsManager,
                VSModuleProjectManager vsProjectManager,
                VSModuleImportExportManager vsModuleImportExportManager,
                VSModuleDependencyManager vsDependencyManager)
        {
            this.vsSettingsManager = vsSettingsManager;
            this.vsProjectManager = vsProjectManager;
            this.vsDependencyManager = vsDependencyManager;
            this.vsModuleImportExportManager = vsModuleImportExportManager;
        }

        public void SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            VSModuleSettingsTO origional = vsSettingsManager.RetrieveModuleSettingsTO();
            if(vsSettingsManager.SaveModuleSettingsTO(to)){
                Logger.Log("VSModule Settings Saved.");
                
                vsProjectManager.UpdateVSProjectsForProjectSettings(origional, to);
                Logger.Log("Visual Studio Projects Updated For Unity Location Change.");
            }
            else
            {
                Logger.LogError("Failure Saving Settings. See Log For Error Information.");
            }
        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            VSModuleSettingsTO to = vsSettingsManager.RetrieveModuleSettingsTO();
            if (to != null)
            {
                Logger.Log("VSModule Settings Loaded.");
            }
            else
            {
                Logger.LogError("Failed To Retrieve Module Settings. See Log For Error Details.");
            }
            return to;
        }

        public void ExportModuleToRepository()
        {
            VSModuleSettingsTO to = RetrieveModuleSettingsTO();
            if (to != null)
            {
                ExportModuleToRepository(to);
            }
            else
            {
                Logger.LogError("Failed to Export Module To Repository. Project Settings Not Found. See Log For Error Details.");
            }
            

        }

        private void ExportModuleToRepository(VSModuleSettingsTO to)
        {
            bool isExported = vsModuleImportExportManager.ExportModule(to);
            if (isExported)
            {
                Logger.Log("VSModule Exported To Repository.");
            }
            else
            {
                Logger.LogError("Failed To Export Module To Repository. See Log For Error Details.");
            }
        }

        public VSModuleDependencyTO RetrieveModuleDependenciesTO()
        {
            VSModuleDependencyTO to = vsDependencyManager.GetDependencyTO();
            if (to == null)
            {
                Logger.LogError("Failed To Retrieve Module Dependencies. See Log For Error Details.");
            }
            else
            {
                Logger.Log("VSModule Dependencies Loaded.");
            }
            
            return to;
        }

        public void AddModuleDependency(String companyShortName, String projectName)
        {
            VSModuleDependencyTO origional = vsDependencyManager.GetDependencyTO();
            bool isAdded = vsDependencyManager.AddDependency(companyShortName, projectName);
            VSModuleSettingsTO settings = vsSettingsManager.RetrieveModuleSettingsTO();
            if (isAdded && settings != null)
            {
                VSModuleDependencyTO updated = vsDependencyManager.GetDependencyTO();
                isAdded = vsProjectManager.UpdateVSProjectsForDependencies(origional, updated, settings);
                if (isAdded)
                {
                    isAdded = vsModuleImportExportManager.ImportModule(companyShortName, projectName, settings);
                    if (isAdded)
                    {
                        Logger.Log("Dependency '" + companyShortName + "' '" + projectName + "' added.");
                    }
                }
            }       
        }
    }
}
