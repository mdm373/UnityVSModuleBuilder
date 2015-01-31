using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleProjectManager
    {
        void UpdateVSProjectsForProjectSettings(VSModuleSettingsTO origional, VSModuleSettingsTO updated);
        bool UpdateVSProjectsForDependencies(VSModuleDependencyTO origional, VSModuleDependencyTO updated, VSModuleSettingsTO settings);
    }

    internal class VSModuleProjectManagerImpl : VSModuleProjectManager
    {
        private const string UNITY_ROOT_PATTERN = @"<UnityRoot>.*</UnityRoot>";
        private const string UNITYROOT_OPEN_TAG = @"<UnityRoot>";
        private const string UNITYROOT_CLOSE_TAG = @"</UnityRoot>";
        private const string DEPENDENCY_ITEM_EDITOR_FORMAT = "\n" +
                                                "    <Reference Include=\"{0}Editor\">\n" +
                                                "        <HintPath>$(SolutionDir)$(PluginsRoot)\\{0}Editor.dll</HintPath>\n" +
                                                "    </Reference>";
        private const string DEPENDENCY_ITEM_FORMAT = "\n" +
                                                "    <Reference Include=\"{0}\">\n" +
                                                "        <HintPath>$(SolutionDir)$(PluginsRoot)\\{0}.dll</HintPath>\n" +
                                                "    </Reference>";
        private const string ITEM_GROUP_HINT_MATCH = "<!--DepRefsGroup--><ItemGroup>";

        private FileSystemController fsController;
        private readonly UnityApi unityApi;

        public VSModuleProjectManagerImpl(UnityApi unityApi, FileSystemController fsController)
        {
            this.fsController = fsController;
            this.unityApi = unityApi;
        }

        public void UpdateVSProjectsForProjectSettings(VSModuleSettingsTO origional, VSModuleSettingsTO updated)
        {
            if (!origional.GetUnityInstallLocation().Equals(updated.GetUnityInstallLocation()))
            {
                FileEntry mainInfo = GetMainProjectFile(origional.GetProjectName());
                FileEntry editorInfo = GetEditorProjectFile(origional.GetProjectName());
                UpdateVSProjUnityLocation(mainInfo, origional.GetProjectName(), updated.GetUnityInstallLocation());
                UpdateVSProjUnityLocation(editorInfo, origional.GetUnityInstallLocation(), updated.GetUnityInstallLocation());
                
            }
        }

        private FileEntry GetEditorProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), @"..\..\VisualStudio\");
            String editorProject = Path.Combine(location, projectName + @"Editor\" + projectName + "Editor.csproj");
            return fsController.GetExistingFile(editorProject);
        }

        private FileEntry GetMainProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), @"..\..\VisualStudio\");
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
                List<VSModuleDependencyItem> toAdd = null;
                List<VSModuleDependencyItem> toRemove = null;
                GetDependenciesToAddAndRemove(out toAdd, out toRemove, origional, updated);
                FileEntry mainProject = GetMainProjectFile(settings.GetProjectName());
                RemoveDependencies(toRemove, mainProject, DEPENDENCY_ITEM_FORMAT);
                AddDependencies(toAdd, mainProject, DEPENDENCY_ITEM_FORMAT);

                FileEntry editorProject = GetEditorProjectFile(settings.GetProjectName());
                RemoveDependencies(toRemove, editorProject, DEPENDENCY_ITEM_FORMAT);
                RemoveDependencies(toRemove, editorProject, DEPENDENCY_ITEM_EDITOR_FORMAT);
                AddDependencies(toAdd, editorProject, DEPENDENCY_ITEM_FORMAT);
                AddDependencies(toAdd, editorProject, DEPENDENCY_ITEM_EDITOR_FORMAT);
                isUpdated = true;
            }
            catch (Exception e)
            {
                unityApi.LogError("Unexpected Exception Updating Visual Studio Project For Depenencies. See Logged Error For Details", e);
            }
            return isUpdated;
        }

        private void AddDependencies(List<VSModuleDependencyItem> toAdd, FileEntry project, String formatString)
        {
            if (toAdd.Count > 0)
            {
                String projectText = project.ReadAllText();
                int itemGroupIndex = projectText.IndexOf(ITEM_GROUP_HINT_MATCH);
                int itemGroupOffset = itemGroupIndex + ITEM_GROUP_HINT_MATCH.Length;

                foreach (VSModuleDependencyItem item in toAdd)
                {
                    String formattedDependencyEntry = String.Format(formatString, item.GetProjectName());
                    projectText = projectText.Insert(itemGroupOffset, formattedDependencyEntry);
                }

                project.WriteAllText(projectText);
            }
        }

        
        

        private void RemoveDependencies(List<VSModuleDependencyItem> toRemove, FileEntry project, String formatString)
        {
            if (toRemove.Count > 0)
            {
                String projectText = project.ReadAllText();
                foreach (VSModuleDependencyItem item in toRemove)
                {
                    String formattedDependencyEntry = String.Format(formatString, item.GetProjectName());
                    projectText = projectText.Replace(formattedDependencyEntry, String.Empty);
                }

                project.WriteAllText(projectText);
            }
            
        }

        private void GetDependenciesToAddAndRemove(out List<VSModuleDependencyItem> toAdd, out List<VSModuleDependencyItem> toRemove,
                VSModuleDependencyTO origional, VSModuleDependencyTO updated)
        {
            toAdd = new List<VSModuleDependencyItem>();
            toRemove = new List<VSModuleDependencyItem>();
            List<VSModuleDependencyItem>.Enumerator updatedDependencies = updated.GetDependencies();
            while (updatedDependencies.MoveNext())
            {
                toAdd.Add(updatedDependencies.Current);
            }
            List<VSModuleDependencyItem>.Enumerator origionalDependencies = origional.GetDependencies();
            while (origionalDependencies.MoveNext())
            {
                if (!toAdd.Contains(origionalDependencies.Current))
                {
                    toRemove.Add(origionalDependencies.Current);
                }
                else
                {
                    toAdd.Remove(origionalDependencies.Current);
                }
            }
        }
    }
}
