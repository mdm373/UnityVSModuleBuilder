using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityVSModuleCommon;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleProjectManager
    {
        void UpdateVSProjectsForProjectSettings(VSModuleSettingsTO original, VSModuleSettingsTO updated);
        bool UpdateVSProjectsForDependencies(VSModuleDependencyTO original, VSModuleDependencyTO updated, VSModuleSettingsTO settings);
    }

    internal class VSModuleProjectManagerImpl : VSModuleProjectManager
    {
        private readonly FileSystemController fsController;
        private readonly UnityApi unityApi;
        
        private readonly List<string> editorFormats = new List<string>();
        private readonly List<string> projectFormats = new List<string>();
        

        public VSModuleProjectManagerImpl(UnityApi unityApi, FileSystemController fsController)
        {
            editorFormats.Add(VSModuleConstants.DEPENDENCY_ITEM_FORMAT);
            editorFormats.Add(VSModuleConstants.DEPENDENCY_ITEM_EDITOR_FORMAT);
            projectFormats.Add(VSModuleConstants.DEPENDENCY_ITEM_FORMAT);
            this.fsController = fsController;
            this.unityApi = unityApi;
        }

        public void UpdateVSProjectsForProjectSettings(VSModuleSettingsTO original, VSModuleSettingsTO updated)
        {
            if (!original.GetUnityInstallLocation().Equals(updated.GetUnityInstallLocation()))
            {
                FileEntry mainInfo = GetMainProjectFile(original.GetProjectName());
                FileEntry editorInfo = GetEditorProjectFile(original.GetProjectName());
                UpdateVSProjUnityLocation(mainInfo, original.GetProjectName(), updated.GetUnityInstallLocation());
                UpdateVSProjUnityLocation(editorInfo, original.GetUnityInstallLocation(), updated.GetUnityInstallLocation());
                
            }
        }

        private FileEntry GetEditorProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.VISUAL_STUDIO_FOLDER);
            String editorProject = Path.Combine(location, projectName + @"Editor\" + projectName + "Editor.csproj");
            return fsController.GetExistingFile(editorProject);
        }

        private FileEntry GetMainProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), VSModuleConstants.VISUAL_STUDIO_FOLDER);
            String mainProject = Path.Combine(location, projectName + @"\" + projectName + ".csproj");
            return fsController.GetExistingFile(mainProject);
        }

        private void UpdateVSProjUnityLocation(FileEntry projectFile, string origionalValue, string updatedValue)
        {
            try
            {
                String projectText = projectFile.ReadAllText();
                Regex expression = new Regex(VSModuleConstants.UNITY_ROOT_PATTERN);
                String replacement = VSModuleConstants.UNITYROOT_OPEN_TAG + updatedValue + VSModuleConstants.UNITYROOT_CLOSE_TAG;
                String replaced = expression.Replace(projectText, replacement);

                projectFile.WriteAllText(replaced);

            }
            catch (Exception e)
            {
                Logger.LogError("Exception Updating visual studio project'" + projectFile.GetFilePath() + '\'', e);
            }
        }


        public bool UpdateVSProjectsForDependencies(VSModuleDependencyTO origional, VSModuleDependencyTO updated, VSModuleSettingsTO settings)
        {
            bool isUpdated = false;
            try
            {
                List<VSModuleDependencyItem> allDepenencies = GetAllDependencies(updated);
                FileEntry mainProject = GetMainProjectFile(settings.GetProjectName());
                AddProjectDependencies(allDepenencies, settings);

                AddEditorDependencies(allDepenencies, settings);

                isUpdated = true;
            }
            catch (Exception e)
            {
                Logger.LogError("Unexpected Exception Updating Visual Studio Project For Depenencies. See Logged Error For Details", e);
            }
            return isUpdated;
        }

        private void AddEditorDependencies(List<VSModuleDependencyItem> allDepenencies, VSModuleSettingsTO settings)
        {
            FileEntry editorProject = GetEditorProjectFile(settings.GetProjectName());
            AddDependencyFormatsToProject(editorProject, allDepenencies, editorFormats);
        }

        private void AddDependencyFormatsToProject(FileEntry projectFile, List<VSModuleDependencyItem> allDepenencies, List<String> formats)
        {
            String projectText = projectFile.ReadAllText();
            int existingDepsStartIndex = projectText.IndexOf(VSModuleConstants.DEPS_START_TAG) + VSModuleConstants.DEPS_START_TAG.Length;
            int existingDepsEndIndex = projectText.IndexOf(VSModuleConstants.DEPS_END_TAG);
            String depsToRemove = projectText.Substring(existingDepsStartIndex, existingDepsEndIndex - existingDepsStartIndex);
            String projectWithNoDeps = projectText.Substring(0, existingDepsStartIndex) + projectText.Substring(existingDepsEndIndex, projectText.Length - existingDepsEndIndex);
            String depsToAdd = String.Empty;
            foreach (VSModuleDependencyItem dep in allDepenencies)
            {
                foreach(String format in formats){
                    String formatted = String.Format(format, dep.GetProjectName());
                    depsToAdd = depsToAdd + formatted;
                }
                
            }
            depsToAdd = depsToAdd + "\n";
            String updatedProjectText = projectWithNoDeps.Insert(existingDepsStartIndex, depsToAdd);
            projectFile.WriteAllText(updatedProjectText);
        }

        private void AddProjectDependencies(List<VSModuleDependencyItem> allDepenencies, VSModuleSettingsTO settings)
        {
            FileEntry project = GetMainProjectFile(settings.GetProjectName());
            AddDependencyFormatsToProject(project, allDepenencies, projectFormats);
        }

        private List<VSModuleDependencyItem> GetAllDependencies(VSModuleDependencyTO dependencyTO)
        {
            List<VSModuleDependencyItem>.Enumerator deps = dependencyTO.GetDependencies();
            List<VSModuleDependencyItem> asList = new List<VSModuleDependencyItem>();
            while (deps.MoveNext())
            {
                asList.Add(deps.Current);
            }
            return asList;
        }

    }
}
