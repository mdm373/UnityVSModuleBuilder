using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleEditor.MiddleTier
{
    internal interface VSModuleUnityManager
    {
        void UpdateUnitySettings(VSModuleSettingsTO existingSettings);
    }

    internal class VSModuleUnityManagerImpl : VSModuleUnityManager
    {
        private readonly UnityApi unityApi;

        public VSModuleUnityManagerImpl(UnityApi unityApi)
        {
            this.unityApi = unityApi;
        }

        public void UpdateUnitySettings(VSModuleSettingsTO settings)
        {
            unityApi.UpdateProjectName(settings.GetProjectName());
            unityApi.UpdateCompanyName(settings.GetCompanyName());
            unityApi.UpdateAndriodBundleIdentifier(GetAndriodBundleIdentifier(settings));
        }

        private string GetAndriodBundleIdentifier(VSModuleSettingsTO settings)
        {
            String companyShortName = GetCleanedString(settings.GetCompanyShortName());
            String projectName = GetCleanedString(settings.GetProjectName());
            return String.Format(VSModuleConstants.ANDRIOD_ID_FORMAT, companyShortName, projectName);
        }

        private String GetCleanedString(String s)
        {
            s = s.Replace(" ", String.Empty);
            s = s.Replace("\n", String.Empty);
            s = s.Replace("\r", String.Empty);
            s = s.Replace("\t", String.Empty);
            s = s.ToLower();
            return s;
        }
    }
}
