using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    public class VSModuleUnityManagerTest
    {
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_COMPANY_NAME = "EXPECTED_COMPANY_NAME";
        private const string EXPECTED_COMPANY_SHORT_NAME = "EXPECTED_COMPANY_SHORT_NAME";
        private const string EXPECTED_ANDRIOD_IDENTIFIER = "com.expected_company_short_name.expected_project_name";

        private UnityApi unityApi;
        private VSModuleUnityManager manager;
        private VSModuleSettingsTO.Builder settingsBuilder;
        private VSModuleSettingsTO requestedSettings;
        
        

        [SetUp]
        public void SetUp()
        {
            this.settingsBuilder = new VSModuleSettingsTO.Builder();
            this.unityApi = Substitute.For<UnityApi>();
            this.manager = new VSModuleUnityManagerImpl(unityApi);
        }

        [Test]
        public void TestUpdateUnityProjectSettings()
        {
            GivenExpectedModuleSettings();
            WhenSettingsUpdateRequested();
            ThenUnityProjectNameChanged();
            ThenUnityCompanyNameChanged();
            ThenAndroidBundleIdentifierUpdated();
        }

        private void ThenAndroidBundleIdentifierUpdated()
        {
            unityApi.Received().UpdateAndriodBundleIdentifier(EXPECTED_ANDRIOD_IDENTIFIER);
        }

        private void ThenUnityCompanyNameChanged()
        {
            unityApi.Received().UpdateCompanyName(EXPECTED_COMPANY_NAME);
        }

        private void ThenUnityProjectNameChanged()
        {
            unityApi.Received().UpdateProjectName(EXPECTED_PROJECT_NAME);
        }

        private void WhenSettingsUpdateRequested()
        {
            requestedSettings = settingsBuilder.Build();
            manager.UpdateUnitySettings(requestedSettings);
        }

        private void GivenExpectedModuleSettings()
        {
            settingsBuilder.ProjectName = EXPECTED_PROJECT_NAME;
            settingsBuilder.CompanyName = EXPECTED_COMPANY_NAME;
            settingsBuilder.CompanyShortName = EXPECTED_COMPANY_SHORT_NAME;
        }
    }
}
