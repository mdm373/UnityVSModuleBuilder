using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor
{
    class VSModuleProjectManager
    {
        private readonly UnityApi unityApi;
        private string UNITY_ROOT_PATTERN = @"<UnityRoot>.*</UnityRoot>";
        private string UNITYROOT_OPEN_TAG = @"<UnityRoot>";
        private string UNITYROOT_CLOSE_TAG = @"</UnityRoot>";

        public VSModuleProjectManager(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void UpdateVisualStudioProjects(VSModuleSettingsXmlModel origional, VSModuleSettingsXmlModel updated)
        {
            if (!origional.unityInstallLocation.Equals(updated.unityInstallLocation))
            {
                String location = Path.Combine(unityApi.GetAssetFolder(), @"..\..\VisualStudio\");
                String mainProject = Path.Combine(location, origional.projectName + @"\" + origional.projectName + ".csproj");
                String editorProject = Path.Combine(location, origional.projectName + @"Editor\" + origional.projectName + "Editor.csproj");
                FileInfo mainInfo = new FileInfo(mainProject);
                FileInfo editorInfo = new FileInfo(editorProject);
                UpdateVSProjUnityLocation(mainInfo, origional.unityInstallLocation, updated.unityInstallLocation);
                UpdateVSProjUnityLocation(editorInfo, origional.unityInstallLocation, updated.unityInstallLocation);
                
            }
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

    }
}
