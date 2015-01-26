using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor
{
    public class VSModuleDelegate
    {
        private readonly VSModuleSettingsManager vsModuleManager;
        private readonly VSModuleProjectManager vsProjectManager;
        private readonly UnityApi unityApi;
        private readonly VSModuleImportManager vsModuleImportExportManager;

        public VSModuleDelegate(UnityApi unityApi, VSModuleXmlSerializer serializer)
        {
            this.vsModuleManager = new VSModuleSettingsManager(unityApi, serializer);
            this.vsProjectManager = new VSModuleProjectManager(unityApi);
            this.vsModuleImportExportManager = new VSModuleImportManager(unityApi);
            this.unityApi = unityApi;
        }

        public void SaveModuleSettingsTO(VSModuleSettingsTO to)
        {
            VSModuleSettingsTO origional = vsModuleManager.RetrieveModuleSettingsTO();
            if(vsModuleManager.SaveModuleSettingsTO(to)){
                unityApi.Log("VSModule Settings Saved.");
                
                vsProjectManager.UpdateVisualStudioProjects(origional.GetXmlModel(), to.GetXmlModel());
                unityApi.Log("Visual Studio Projects Updated For Unity Location Change.");
            }
        }

        public VSModuleSettingsTO RetrieveModuleSettingsTO()
        {
            VSModuleSettingsTO to = vsModuleManager.RetrieveModuleSettingsTO();
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

        

    }
}
