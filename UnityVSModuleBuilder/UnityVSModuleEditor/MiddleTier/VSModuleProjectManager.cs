using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    internal class VSModuleProjectManager
    {
        private readonly UnityApi unityApi;
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
        private const string ITEM_GROUP_MATCH = "<ItemGroup>";

        public VSModuleProjectManager(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void UpdateVisualStudioProjects(VSModuleSettingsXmlModel origional, VSModuleSettingsXmlModel updated)
        {
            if (!origional.unityInstallLocation.Equals(updated.unityInstallLocation))
            {
                FileInfo mainInfo = GetMainProjectFile(origional.projectName);
                FileInfo editorInfo = GetEditorProjectFile(origional.projectName);
                UpdateVSProjUnityLocation(mainInfo, origional.unityInstallLocation, updated.unityInstallLocation);
                UpdateVSProjUnityLocation(editorInfo, origional.unityInstallLocation, updated.unityInstallLocation);
                
            }
        }

        private FileInfo GetEditorProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), @"..\..\VisualStudio\");
            String editorProject = Path.Combine(location, projectName + @"Editor\" + projectName + "Editor.csproj");
            return new FileInfo(editorProject);
        }

        private FileInfo GetMainProjectFile(String projectName)
        {
            String location = Path.Combine(unityApi.GetAssetFolder(), @"..\..\VisualStudio\");
            String mainProject = Path.Combine(location, projectName + @"\" + projectName + ".csproj");
            return new FileInfo(mainProject);
        }

        private void UpdateVSProjUnityLocation(FileInfo projectFile, string origionalValue, string updatedValue)
        {
            try
            {
                String projectText = File.ReadAllText(projectFile.FullName);
                Regex expression = new Regex(UNITY_ROOT_PATTERN);
                String replacement = UNITYROOT_OPEN_TAG + updatedValue + UNITYROOT_CLOSE_TAG;
                String replaced = expression.Replace(projectText, replacement);

                File.WriteAllText(projectFile.FullName, replaced);

            }
            catch (Exception e)
            {
                unityApi.LogError("Exception Updating visual studio project'" + projectFile.FullName + '\'');
                unityApi.LogException(e);
            }
        }


        internal bool UpdateVisualStudioProjects(VSModuleDependencyTO origional, VSModuleDependencyTO updated, VSModuleSettingsTO settings)
        {
            bool isUpdated = false;
            try
            {
                List<VSModuleDependencyItem> toAdd = null;
                List<VSModuleDependencyItem> toRemove = null;
                GetDependenciesToAddAndRemove(out toAdd, out toRemove, origional, updated);
                FileInfo mainProject = GetMainProjectFile(settings.GetProjectName());
                RemoveDependencies(toRemove, mainProject, DEPENDENCY_ITEM_FORMAT);
                AddDependencies(toAdd, mainProject, DEPENDENCY_ITEM_FORMAT);

                FileInfo editorProject = GetEditorProjectFile(settings.GetProjectName());
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

        private void AddDependencies(List<VSModuleDependencyItem> toAdd, FileInfo project, String formatString)
        {
            if (toAdd.Count > 0)
            {
                String projectText = File.ReadAllText(project.FullName);
                int itemGroupIndex = projectText.IndexOf(ITEM_GROUP_MATCH);
                int itemGroupOffset = itemGroupIndex + ITEM_GROUP_MATCH.Length;

                foreach (VSModuleDependencyItem item in toAdd)
                {
                    String formattedDependencyEntry = String.Format(formatString, item.GetProjectName());
                    projectText = projectText.Insert(itemGroupOffset, formattedDependencyEntry);
                }

                File.WriteAllText(project.FullName, projectText);
            }
        }

        
        

        private void RemoveDependencies(List<VSModuleDependencyItem> toRemove, FileInfo project, String formatString)
        {
            if (toRemove.Count > 0)
            {
                String projectText = File.ReadAllText(project.FullName);
                foreach (VSModuleDependencyItem item in toRemove)
                {
                    String formattedDependencyEntry = String.Format(formatString, item.GetProjectName());
                    projectText = projectText.Replace(formattedDependencyEntry, String.Empty);
                }

                File.WriteAllText(project.FullName, projectText);
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
