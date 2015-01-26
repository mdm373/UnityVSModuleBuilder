using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor
{
    public class VSModuleSettingsTO
    {
        private readonly VSModuleSettingsXmlModel model;

        public VSModuleSettingsTO(VSModuleSettingsXmlModel model)
        {
            this.model = model;
        }

        public string GetProjectName()
        {
            return model.projectName;
        }

        public string GetCompanyName()
        {
            return model.companyName;
        }

        public string GetCompanyShortName()
        {
            return model.companyShortName;
        }

        public string GetRepoLocation()
        {
            return model.repoLocation;
        }

        public void SetRepoLocation(string p)
        {
            model.repoLocation = p;
        }

        public string GetUnityInstallLocation()
        {
            return model.unityInstallLocation;
        }

        public void SetUnityInstallLocation(string p)
        {
            model.unityInstallLocation = p;
        }

        public VSModuleSettingsXmlModel GetXmlModel()
        {
            return model;
        }
    }
}
