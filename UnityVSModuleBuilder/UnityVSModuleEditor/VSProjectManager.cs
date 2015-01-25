using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.MSBuildProcess;
using UnityVSModuleEditor.UnityApis;

namespace UnityVSModuleEditor
{
    class VSProjectManager
    {
        private readonly UnityApi unityApi;

        public VSProjectManager(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void UpdateVisualStudioProjects(VSModuleSettingsXMLModel origional, VSModuleSettingsXMLModel updated)
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
                unityApi.Log("Visual Studio Projects Updated For Unity Location Change.");
            }
        }

        private void UpdateVSProjUnityLocation(FileInfo projectFile, string origionalValue, string updatedValue)
        {
            FileStream fileStream = null;
            Project project = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Project));
                fileStream = new FileStream(projectFile.FullName, FileMode.Open);
                project = (Project)serializer.Deserialize(fileStream);
                StreamUtil.CloseFileStream(fileStream, unityApi);
                project.propertyGroup1.unityRoot.text = updatedValue;
                project.propertyGroup2.unityRoot.text = updatedValue;
                project.propertyGroup3.unityRoot.text = updatedValue;
                fileStream = new FileStream(projectFile.FullName, FileMode.Create);
                serializer.Serialize(fileStream, project);
            }
            catch (Exception e)
            {
                unityApi.LogError("Exception Updating visual studio project'" + projectFile.FullName + '\'');
                unityApi.LogException(e);
            }
            finally
            {
                StreamUtil.CloseFileStream(fileStream, unityApi);
            }
        }

    }
}
