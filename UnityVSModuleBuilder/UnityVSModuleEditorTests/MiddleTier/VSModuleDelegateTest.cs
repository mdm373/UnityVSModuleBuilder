using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityVSModuleCommon;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    class VSModuleDelegateTest
    {
        private const string EXPECTED_COMPANY_SHORT_NAME= "EXPECTED_COMPANY_SHORT_NAME";
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXISTING_DEP_COMAPNY_NAME = "EXISTING_DEP_COMAPNY_NAME";
        private const string EXISTING_DEP_PROJECT_NAME = "EXISTING_DEP_PROJECT_NAME";
        private const string UNKOWN_PROJECT_NAME = "UNKOWN_PROJECT_NAME";
        private const string UKNOWN_COMPANY_SHORT_NAME = "UKNOWN_COMPANY_SHORT_NAME";

        private VSModuleSettingsManager vsSettingsManager;
        private VSModuleProjectManager vsProjectManager;
        private VSModuleImportExportManager vsImportExportManager;
        private VSModuleDependencyManager vsDependencyManager;
        
        private VSModuleDelegate vsModuleDelegate;
        private VSModuleSettingsTO to;
        private VSModuleSettingsTO existingSettings;
        private LoggingService loggingService;
        private VSModuleSettingsTO retrievedSettings;
        private VSModuleDependencyTO existingDependencyTO;
        private VSModuleDependencyTO retrievedDependencyTO;
        private VSModuleDependencyTO updatedDepdencyTO;
        private List<VSModuleDependencyItem> toUpdateList;
        private VSModuleDependencyItem existingDependency;
        private VSModuleDependencyItem unknownDependency;
        private List<VSModuleDependencyItem> toRemoveList;
        private VSModuleUnityManager vsUnityManager;
        
        [SetUp]
        public void SetUp()
        {
            existingDependency = new VSModuleDependencyItem(EXISTING_DEP_COMAPNY_NAME, EXISTING_DEP_PROJECT_NAME);
            toRemoveList = new List<VSModuleDependencyItem>();
            toUpdateList = new List<VSModuleDependencyItem>();
            loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            vsUnityManager = Substitute.For<VSModuleUnityManager>();
            vsSettingsManager = Substitute.For<VSModuleSettingsManager>();
            vsProjectManager = Substitute.For<VSModuleProjectManager>();
            vsImportExportManager = Substitute.For<VSModuleImportExportManager>();
            vsDependencyManager = Substitute.For<VSModuleDependencyManager>();

            vsModuleDelegate = new VSModuleDelegateImpl(vsSettingsManager, 
                vsProjectManager, 
                vsImportExportManager, 
                vsDependencyManager,
                vsUnityManager);
        }

        [Test]
        public void TestSaveModuleSettingsSuccess()
        {
            GivenDependencyTO();
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenSettingsManagerSaveSuccessful();
            WhenSettingsSaveRequested();
            ThenSettingsManagerSaveRequestedWithTO();
            ThenProjectManagerSetingsUpdateRequested();
        }

        [Test]
        public void TestSaveModuleSettingsFailure()
        {
            GivenDependencyTO();
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenSettingsManagerSaveFailure();
            WhenSettingsSaveRequested();
            ThenProjectManagerSettingsUpdateNotRequested();
            ThenErrorLogged();
        }

        [Test]
        public void TestExportModuleSuccess()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenExportManagerExportsSuccessfully();
            WhenExportRequested();
            ThenExportManagerExportRequestedWithSettings();
            ThenLoggerLogsSuccessMessage();
        }

        [Test]
        public void TestExportModuleFailure()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenExportManagerExportsWithFailure();
            WhenExportRequested();
            ThenErrorLogged();
        }

        [Test]
        public void TestExportButNoSettingsFound()
        {
            GivenSettingsManagerProvidesNoExstingSettingsTO();
            WhenExportRequested();
            ThenExportManagerExportNotRequested();
            ThenErrorLogged();
        }

        [Test]
        public void TestRetrieveSettingsTOSuccess()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            WhenRetrieveSettingsTORequested();
            ThenExistingSettingsAreRetrieved();
        }

        [Test]
        public void TestRetrievedSettingsTOFailure()
        {
            GivenSettingsManagerProvidesNoExstingSettingsTO();
            WhenRetrieveSettingsTORequested();
            ThenNullSettingsTORetrieved();
            ThenErrorLogged();
        }

        [Test]
        public void TestRetrieveModuleDependenciesSuccess()
        {
            GivenDependencyManagerProvidesExistingDependencies();
            WhenRetrieveDependencyTORequested();
            ThenExistingDependencyTORetrieved();
        }

        [Test]
        public void TestRetrieveModuleDepndenciesFailure()
        {
            GivenDependencyManagerProvidesNoExistingDependencies();
            WhenRetrieveDependencyTORequested();
            ThenNullDependencyTORetrieved();
            ThenErrorLogged();
        }

        [Test]
        public void TestDependencyAddSuccess()
        {
            GivenDependencyManagerProvidesExistingDependenciesThenUpdatedDependencies();
            GivenDependencyManagerSuccessfullyAddsDependencies();
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenProjectManagerSuccessfulyUpdatesForDependencyChange();
            WhenDependencyAddRequested();
            ThenDependencyManagerAddsDependency();
            ThenProjectManagerUpdatesForDependencyChange();
            ThenImportManagerImportsDependencyModule(); 
        }

        [Test]
        public void TestDependencyUpdate()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenDependencyManagerProvidesExistingDependencies();
            GivenProvidedExistingDependenciesToUpdate();
            WhenDependencyUpdateRequested();
            ThenImportManagerImportRequestedForEachDependency();
        }

        [Test]
        public void TestDependencyUpdateError()
        {
            GivenSettingsManagerRetrievalThrowsException();
            WhenDependencyUpdateRequested();
            ThenImportManagerImportNotRequested();
            ThenErrorWithExceptionLogged();
        }

        [Test]
        public void TestUnknownDependencyUpdate()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenDependencyManagerProvidesExistingDependencies();
            GivenProvidedUnknownDependenceisToUpdate();
            WhenDependencyUpdateRequested();
            ThenImportManagerImportNotRequested();
            ThenErrorLogged();
        }

        [Test]
        public void TestDependencyRemoved()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            GivenDependencyManagerProvidesExistingDependenciesThenUpdatedDependencies();
            GivenExistingDependencyInRemoveList();
            WhenDependencyRemoveRequested();
            ThenDependencyManagerRemovesDependency();
            ThenProjectManagerUpdatesForDependencyChange();
            
        }

        [Test]
        public void TestApplyProjectSettings()
        {
            GivenSettingsManagerProvidesExistingSettingsTO();
            WhenUpdateUnitySettingsRequested();
            ThenUnityManagerSettingsUpdateRequested();
        }

        private void ThenUnityManagerSettingsUpdateRequested()
        {
            vsUnityManager.Received().UpdateUnitySettings(existingSettings);
        }

        private void WhenUpdateUnitySettingsRequested()
        {
            vsModuleDelegate.UpdateUnitySettings();
        }

        private void GivenExistingDependencyInRemoveList()
        {
            toRemoveList.Add(existingDependency);
        }

        private void WhenDependencyRemoveRequested()
        {
            vsModuleDelegate.RemoveDependencies(toRemoveList.GetEnumerator());
        }

        private void ThenDependencyManagerRemovesDependency()
        {
            vsDependencyManager.Received().RemoveDependencies(Arg.Any<List<VSModuleDependencyItem>.Enumerator>());
        }

        private void GivenProvidedUnknownDependenceisToUpdate()
        {
            unknownDependency = new VSModuleDependencyItem(UKNOWN_COMPANY_SHORT_NAME, UNKOWN_PROJECT_NAME);
            toUpdateList.Add(unknownDependency);
        }

        private void ThenImportManagerImportNotRequested()
        {
            vsImportExportManager.DidNotReceive().ImportModule(Arg.Any<String>(), Arg.Any<string>(), Arg.Any<VSModuleSettingsTO>());
        }

        private void GivenSettingsManagerRetrievalThrowsException()
        {
            this.vsSettingsManager.When(x => x.RetrieveModuleSettingsTO()).Do(x => { throw new Exception(); });
        }

        private void GivenProvidedExistingDependenciesToUpdate()
        {
            toUpdateList.Add(existingDependency);
        }

        private void ThenImportManagerImportRequestedForEachDependency()
        {
            vsImportExportManager.Received().ImportModule(EXISTING_DEP_COMAPNY_NAME, EXISTING_DEP_PROJECT_NAME, existingSettings);
        }

        private void WhenDependencyUpdateRequested()
        {
            vsModuleDelegate.UpdateModuleDependencies(toUpdateList.GetEnumerator());
        }

        private void ThenDependencyManagerAddsDependency()
        {
            this.vsDependencyManager.Received().AddDependency(EXPECTED_COMPANY_SHORT_NAME, EXPECTED_PROJECT_NAME);
        }

        private void ThenProjectManagerUpdatesForDependencyChange()
        {
            vsProjectManager.Received().UpdateVSProjectsForDependencies(existingDependencyTO, updatedDepdencyTO, existingSettings);
        }

        private void ThenImportManagerImportsDependencyModule()
        {
            vsImportExportManager.Received().ImportModule(EXPECTED_COMPANY_SHORT_NAME, EXPECTED_PROJECT_NAME, existingSettings);
        }

        private void WhenDependencyAddRequested()
        {
            this.vsModuleDelegate.AddModuleDependency(EXPECTED_COMPANY_SHORT_NAME, EXPECTED_PROJECT_NAME);
        }

        private void GivenProjectManagerSuccessfulyUpdatesForDependencyChange()
        {
            vsProjectManager.UpdateVSProjectsForDependencies(existingDependencyTO, updatedDepdencyTO, existingSettings).Returns(true);
        }

        private void GivenDependencyManagerSuccessfullyAddsDependencies()
        {
            vsDependencyManager.AddDependency(EXPECTED_COMPANY_SHORT_NAME, EXPECTED_PROJECT_NAME).Returns(true);
        }

        private void ThenNullDependencyTORetrieved()
        {
            Assert.Null(retrievedDependencyTO);
        }

        private void GivenDependencyManagerProvidesNoExistingDependencies()
        {
            VSModuleDependencyTO nullTO = null;
            this.vsDependencyManager.GetDependencyTO().Returns(nullTO);
        }
        private void ThenExistingDependencyTORetrieved()
        {
            Assert.AreSame(existingDependencyTO, retrievedDependencyTO);
        }

        private void WhenRetrieveDependencyTORequested()
        {
            retrievedDependencyTO = this.vsModuleDelegate.RetrieveModuleDependenciesTO();
        }

        private void GivenDependencyManagerProvidesExistingDependencies()
        {
            List<VSModuleDependencyItem> depList = new List<VSModuleDependencyItem>();
            depList.Add(existingDependency);
            existingDependencyTO = new VSModuleDependencyTO(depList);
            this.vsDependencyManager.GetDependencyTO().Returns(existingDependencyTO);
        }

        private void GivenDependencyManagerProvidesExistingDependenciesThenUpdatedDependencies(){
            updatedDepdencyTO = new VSModuleDependencyTO(new List<VSModuleDependencyItem>());
            existingDependencyTO = new VSModuleDependencyTO(new List<VSModuleDependencyItem>());
            this.vsDependencyManager.GetDependencyTO().Returns(existingDependencyTO, updatedDepdencyTO);
        }

        private void ThenNullSettingsTORetrieved()
        {
            Assert.Null(retrievedSettings);
        }

        private void ThenExistingSettingsAreRetrieved()
        {
            Assert.AreSame(existingSettings, retrievedSettings);
        }

        private void WhenRetrieveSettingsTORequested()
        {
            retrievedSettings = this.vsModuleDelegate.RetrieveModuleSettingsTO();
        }

        private void ThenExportManagerExportNotRequested()
        {
            this.vsImportExportManager.DidNotReceive().ExportModule(Arg.Any<VSModuleSettingsTO>());
        }

        private void GivenSettingsManagerProvidesNoExstingSettingsTO()
        {
            VSModuleSettingsTO nullTO = null;
            vsSettingsManager.RetrieveModuleSettingsTO().Returns(nullTO);
        }

        private void GivenExportManagerExportsWithFailure()
        {
            this.vsImportExportManager.ExportModule(existingSettings).Returns(false);
        }

        private void ThenLoggerLogsSuccessMessage()
        {
            loggingService.Received().Log(Arg.Any<String>());
        }

        private void GivenExportManagerExportsSuccessfully()
        {
            this.vsImportExportManager.ExportModule(existingSettings).Returns(true);
        }

        private void ThenExportManagerExportRequestedWithSettings()
        {
            this.vsImportExportManager.Received().ExportModule(existingSettings);
        }

        private void WhenExportRequested()
        {
            vsModuleDelegate.ExportModuleToRepository();
        }

        private void ThenErrorLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>());
        }

        private void ThenErrorWithExceptionLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>(), Arg.Any<Exception>());
        }

        private void ThenProjectManagerSettingsUpdateNotRequested()
        {
            vsProjectManager.DidNotReceive().UpdateVSProjectsForProjectSettings(Arg.Any<VSModuleSettingsTO>(), Arg.Any<VSModuleSettingsTO>());
        }

        private void GivenSettingsManagerSaveFailure()
        {
            vsSettingsManager.SaveModuleSettingsTO(to).Returns(false);
        }

        private void GivenSettingsManagerSaveSuccessful()
        {
            vsSettingsManager.SaveModuleSettingsTO(to).Returns(true);
        }

        private void ThenProjectManagerSetingsUpdateRequested()
        {
            vsProjectManager.Received().UpdateVSProjectsForProjectSettings(existingSettings, to);
        }

        private void ThenSettingsManagerSaveRequestedWithTO()
        {
            vsSettingsManager.Received().SaveModuleSettingsTO(to);
        }

        private void WhenSettingsSaveRequested()
        {
            vsModuleDelegate.SaveModuleSettingsTO(to);
        }

        private void GivenSettingsManagerProvidesExistingSettingsTO()
        {
            existingSettings = new VSModuleSettingsTO.Builder().Build();
            vsSettingsManager.RetrieveModuleSettingsTO().Returns(existingSettings);
        }

        private void GivenDependencyTO()
        {
            to = new VSModuleSettingsTO.Builder().Build();
        }
    }
}
