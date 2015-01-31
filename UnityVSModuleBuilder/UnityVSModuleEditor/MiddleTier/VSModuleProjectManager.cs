using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        private const string VISUAL_STUDIO_FOLDER = @"..\..\VisualStudio\";
        private const string UNITY_ROOT_PATTERN = @"<UnityRoot>.*</UnityRoot>";
        private const string UNITYROOT_OPEN_TAG = @"<UnityRoot>";
        private const string UNITYROOT_CLOSE_TAG = @"</UnityRoot>";
        private const string DEPENDENCY_ITEM_EDITOR_FORMAT = 
@"
<Reference Include='{0}Editor'>
    <HintPath>$(SolutionDir)$(PluginsEditorRoot)\{0}Editor.dll</HintPath>
</Reference>";
        private const string DEPENDENCY_ITEM_FORMAT = 
@"
<Reference Include='{0}'>
    <HintPath>$(SolutionDir)$(PluginsRoot)\{0}.dll</HintPath>
</Reference>";
        private const string ITEM_GROUP_HINT_MATCH = "<!--DepRefsGroup--><ItemGroup>";

        private readonly FileSystemController fsController;
        private readonly UnityApi unityApi;
        private const string DEPS_START_TAG = "<!--DepRefsGroup-->";
        private const string DEPS_END_TAG = "<!--EndDepRefsGroup-->";
        private readonly List<string> editorFormats = new List<string>();
        private readonly List<string> projectFormats = new List<string>();
        

        public VSModuleProjectManagerImpl(UnityApi unityApi, FileSystemController fsController)
        {
            editorFormats.Add(DEPENDENCY_ITEM_FORMAT);
            editorFormats.Add(DEPENDENCY_ITEM_EDITOR_FORMAT);
            projectFormats.Add(DEPENDENCY_ITEM_FORMAT);
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
            String location = Path.Combine(unityApi.GetAssetFolder(), VISUAL_STUDIO_FOLDER);
            String editorProject = Path.Combine(location, projectName + @"Editor\" + projectName + "Editor.csproj");
            return fsController.GetExistingFile(editorProject);
        }

        private FileEntry GetMainProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), VISUAL_STUDIO_FOLDER);
            String mainProject = Path.Combine(location, projectName + @"\" + projectName + ".csproj");
            return fsController.GetExistingFile(mainProject);
        }

        private void UpdateVSProjUnityLocation(FileEntry projectFile, string origionalValue, string updatedValue)
        {
            try
            {
                String projectText = projectFile.ReadAllText();
                Regex expression = new Regex(UNITY_ROOT_PATTERN);
                String replacement = UNITYROOT_OPEN_TAG + updatedValue + UNITYROOT_CLOSE_TAG;
                String replaced = expression.Replace(projectText, replacement);

                projectFile.WriteAllText(replaced);

            }
            catch (Exception e)
            {
                unityApi.LogError("Exception Updating visual studio project'" + projectFile.GetFilePath() + '\'');
                unityApi.LogException(e);
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
                unityApi.LogError("Unexpected Exception Updating Visual Studio Project For Depenencies. See Logged Error For Details", e);
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
            int existingDepsStartIndex = projectText.IndexOf(DEPS_START_TAG) + DEPS_START_TAG.Length;
            int existingDepsEndIndex = projectText.IndexOf(DEPS_END_TAG);
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
