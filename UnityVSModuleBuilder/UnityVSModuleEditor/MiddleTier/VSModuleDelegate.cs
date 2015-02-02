using System;
using System.Collections.Generic;
using UnityVSModuleCommon;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleDelegate
    {
        void SaveModuleSettingsTO(VSModuleSettingsTO to);
        void ExportModuleToRepository();
        VSModuleSettingsTO RetrieveModuleSettingsTO();
        VSModuleDependencyTO RetrieveModuleDependenciesTO();
        void AddModuleDependency(String companyShortName, String projectName);
        void UpdateModuleDependencies(System.Collections.Generic.List<VSModuleDependencyItem>.Enumerator enumerator);

        void RemoveDependencies(List<VSModuleDependencyItem>.Enumerator enumerator);

        void UpdateUnitySettings();
    }

    internal class VSModuleDelegateImpl : VSModuleDelegate
    {
        private readonly VSModuleSettingsManager vsSettingsManager;
        private readonly VSModuleProjectManager vsProjectManager;
        private readonly VSModuleImportExportManager vsImportExportManager;
        private readonly VSModuleDependencyManager vsDependencyManager;
        private readonly VSModuleUnityManager vsUnityManager;

        public VSModuleDelegateImpl(VSModuleSettingsManager vsSettingsManager,
                VSModuleProjectManager vsProjectManager,
                VSModuleImportExportManager vsModuleImportExportManager,
                VSModuleDependencyManager vsDependencyManager,
                VSModuleUnityManager unityManager)
        {
            this.vsSettingsManager = vsSettingsManager;
            this.vsProjectManager = vsProjectManager;
            this.vsDependencyManager = vsDependencyManager;
            this.vsImportExportManager = vsModuleImportExportManager;
            this.vsUnityManager = unityManager;
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
            bool isExported = vsImportExportManager.ExportModule(to);
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
                    isAdded = vsImportExportManager.ImportModule(companyShortName, projectName, settings);
                    if (isAdded)
                    {
                        Logger.Log("Dependency '" + companyShortName + "' '" + projectName + "' added.");
                    }
                }
            }       
        }

        public void UpdateModuleDependencies(List<VSModuleDependencyItem>.Enumerator enumerator)
        {
            try
            {
                VSModuleSettingsTO settings = vsSettingsManager.RetrieveModuleSettingsTO();
                VSModuleDependencyTO existingDependencies = vsDependencyManager.GetDependencyTO();
                while (enumerator.MoveNext())
                {
                    VSModuleDependencyItem item = enumerator.Current;
                    if (existingDependencies.ContainsMatchingItem(item))
                    {
                        vsImportExportManager.ImportModule(item.GetCompanyShortName(), item.GetProjectName(), settings);
                    }
                    else
                    {
                        Logger.LogError("Cannot add dependency item '" + item.GetDisplayValue() + "'. Current project does not depend on item.");
                    }
                }
                Logger.Log("VSModule Dependencies Updated.");
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected exception updating dependencies for project. See log for error details.", e);
            }
        }


        public void RemoveDependencies(List<VSModuleDependencyItem>.Enumerator enumerator)
        {
            try
            {
                VSModuleSettingsTO settings = vsSettingsManager.RetrieveModuleSettingsTO();
                VSModuleDependencyTO original = vsDependencyManager.GetDependencyTO();
                vsDependencyManager.RemoveDependencies(enumerator); //Enumerator Consumed
                VSModuleDependencyTO updated = vsDependencyManager.GetDependencyTO();
                vsProjectManager.UpdateVSProjectsForDependencies(original, updated, settings);
                Logger.Log("VSModule Dependency References Removed. (Assets Not Removed From Project)");
            } catch(Exception e){
                Logger.LogError("Unexpected exception removing dependencies for project. See log for error details.", e);
            }
        }



        public void UpdateUnitySettings()
        {
            try
            {
                VSModuleSettingsTO settings = vsSettingsManager.RetrieveModuleSettingsTO();
                vsUnityManager.UpdateUnitySettings(settings);
                Logger.Log("VSModule Settings Applied to Unity Project.");
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected exception updating unity project settings for VSModule settings. See log for error details.", e);
            }
        }
    }
}
