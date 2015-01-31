using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    public class VSModuleSettingsManagerTest
    {
        private const string ASSET_FOLDER = "ASSET_FOLDER";
        private const string SETTINGS_FILE_LOCATION = @"ASSET_FOLDER\Editor\ModuleConfig.xml";
        private const string EXPECTED_COMPANY_NAME = "EXPECTED_COMPANY_NAME";
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_COMPANY_SHORT_NAME = "EXPECTED_COMPANY_SHORT_NAME";
        private const string EXPECTED_UNITY_INSTALL_LOCATION = "EXPECTED_UNITY_INSTALL_LOCATION";
        private const string EXPECTED_REPO_LOCATION = "EXPECTED_REPO_LOCATION";

        private UnityApi unityApi;
        private XmlSerializerWrapper serializer;
        private FileSystemController fsController;
        private VSModuleSettingsManagerImpl manager;
        private FileEntry settingsFile;
        private VSModuleSettingsXmlModel settingsModel;
        private VSModuleSettingsTO retrieved;
        private VSModuleSettingsTO.Builder builder;
        private bool isSaveSuccess;
        
        

        [SetUp]
        public void SetUp()
        {
            builder = new VSModuleSettingsTO.Builder();
            settingsModel = Substitute.For<VSModuleSettingsXmlModel>();
            settingsFile = Substitute.For<FileEntry>();
            unityApi = Substitute.For<UnityApi>();
            serializer = Substitute.For<XmlSerializerWrapper>();
            fsController = Substitute.For<FileSystemController>();
            manager = new VSModuleSettingsManagerImpl(unityApi, serializer, fsController);
        }

        [Test]
        public void TestRetrieveModuleSettingsSuccess()
        {
            GivenUnityAPIHasAssetFolder();
            GivenXmlSettingsFileExists();
            GivenSerializerHasModelForSettingsFile();
            GivenModelHasExpectedSetUp();
            WhenRetrieveSettingsRequested();
            ThenSettingsFileMatchesModel();
        }

        [Test]
        public void TestRetrieveModelSettingsFileMissing()
        {
            GivenUnityAPIHasAssetFolder();
            GivenXmlFileDoesNotExist();
            WhenRetrieveSettingsRequested();
            ThenSerializerNotRequested();
            ThenRetrievedTOIsEmpty();
        }

        [Test]
        public void TestSaveModelSettings()
        {
            GivenUnityAPIHasAssetFolder();
            GivenXmlSettingsFileExists();
            GivenExpectedRequestedTO();
            WhenSaveSettingsRequested();
            ThenSavedModelMathesTO();
            ThenSaveIsSuccess();
        }

        private void ThenSaveIsSuccess()
        {
            Assert.True(isSaveSuccess);
        }

        private void ThenSavedModelMathesTO()
        {
            serializer.Received().SerializeToFile<VSModuleSettingsXmlModel>(settingsFile, Arg.Is<VSModuleSettingsXmlModel>(x => ValidateSavedModel(x)));
        }

        private bool ValidateSavedModel(VSModuleSettingsXmlModel model)
        {
            Assert.AreEqual(EXPECTED_COMPANY_NAME, model.companyName);
            Assert.AreEqual(EXPECTED_COMPANY_SHORT_NAME, model.companyShortName);
            Assert.AreEqual(EXPECTED_PROJECT_NAME, model.projectName);
            Assert.AreEqual(EXPECTED_UNITY_INSTALL_LOCATION, model.unityInstallLocation);
            Assert.AreEqual(EXPECTED_REPO_LOCATION, model.repoLocation);
            return true;
        }

        private void WhenSaveSettingsRequested()
        {
            isSaveSuccess = manager.SaveModuleSettingsTO(builder.Build());
        }

        private void GivenExpectedRequestedTO()
        {
            builder.CompanyName = EXPECTED_COMPANY_NAME;
            builder.CompanyShortName = EXPECTED_COMPANY_SHORT_NAME;
            builder.ProjectName = EXPECTED_PROJECT_NAME;
            builder.RepoLocation = EXPECTED_REPO_LOCATION;
            builder.UnityInstallLocation = EXPECTED_UNITY_INSTALL_LOCATION;
        }

        private void ThenRetrievedTOIsEmpty()
        {
            Assert.AreEqual(String.Empty, retrieved.GetCompanyName());
            Assert.AreEqual(String.Empty, retrieved.GetCompanyShortName());
            Assert.AreEqual(String.Empty, retrieved.GetProjectName());
            Assert.AreEqual(String.Empty, retrieved.GetUnityInstallLocation());
            Assert.AreEqual(String.Empty, retrieved.GetRepoLocation());
        }

        private void ThenSerializerNotRequested()
        {
            serializer.DidNotReceive().GetDeserialized<VSModuleSettingsXmlModel>(Arg.Any<FileEntry>());
        }

        private void GivenXmlFileDoesNotExist()
        {
            FileEntry nullFile = null;
            fsController.GetExistingFile(SETTINGS_FILE_LOCATION).Returns(nullFile);
        }

        private void GivenModelHasExpectedSetUp()
        {
            settingsModel.companyName = EXPECTED_COMPANY_NAME;
            settingsModel.projectName = EXPECTED_PROJECT_NAME;
            settingsModel.companyShortName = EXPECTED_COMPANY_SHORT_NAME;
            settingsModel.unityInstallLocation = EXPECTED_UNITY_INSTALL_LOCATION;
            settingsModel.repoLocation = EXPECTED_REPO_LOCATION;
        }

        private void GivenUnityAPIHasAssetFolder()
        {
            unityApi.GetAssetFolder().Returns(ASSET_FOLDER);
        }

        private void ThenSettingsFileMatchesModel()
        {
            Assert.AreEqual(EXPECTED_COMPANY_NAME, retrieved.GetCompanyName());
            Assert.AreEqual(EXPECTED_COMPANY_SHORT_NAME, retrieved.GetCompanyShortName());
            Assert.AreEqual(EXPECTED_PROJECT_NAME, retrieved.GetProjectName());
            Assert.AreEqual(EXPECTED_UNITY_INSTALL_LOCATION, retrieved.GetUnityInstallLocation());
            Assert.AreEqual(EXPECTED_REPO_LOCATION, retrieved.GetRepoLocation());
        }

        private void WhenRetrieveSettingsRequested()
        {
            retrieved = manager.RetrieveModuleSettingsTO();
        }

        private void GivenSerializerHasModelForSettingsFile()
        {
            serializer.GetDeserialized<VSModuleSettingsXmlModel>(settingsFile).Returns(settingsModel);
        }

        private void GivenXmlSettingsFileExists()
        {
            settingsFile.IsPresent().Returns(true);
            fsController.GetExistingFile(SETTINGS_FILE_LOCATION).Returns(settingsFile);
            fsController.GetExistingOrNewlyCreatedFile(SETTINGS_FILE_LOCATION).Returns(settingsFile);
        }

    }
}
