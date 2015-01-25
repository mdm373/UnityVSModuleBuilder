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
    public class VSModuleDelegate
    {
        private VSModuleSettingsManager vsModuleManager;
        private VSProjectManager vsProjectManager;
        private UnityApi unityApi;

        public VSModuleDelegate(UnityApi unityApi)
        {
            this.vsModuleManager = new VSModuleSettingsManager(unityApi);
            this.vsProjectManager = new VSProjectManager(unityApi);
            this.unityApi = unityApi;
        }

        public void SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            VSModuleSettingsTO origional = vsModuleManager.RetrieveModuleSettingsTO();
            if(vsModuleManager.SaveModuleSettingsTO(to)){
                vsProjectManager.UpdateVisualStudioProjects(origional.GetXmlModel(), to.GetXmlModel());
                unityApi.Log("VSModule Settings Saved.");
            }

        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            return vsModuleManager.RetrieveModuleSettingsTO();
        }

        public void ExportModuleToRepository()
        {
            String repoLocation = vsModuleManager.RetrieveModuleSettingsTO().GetRepoLocation();
            //unityApi.ExportRootAssets(repoLocation);
        }

        

    }
}
