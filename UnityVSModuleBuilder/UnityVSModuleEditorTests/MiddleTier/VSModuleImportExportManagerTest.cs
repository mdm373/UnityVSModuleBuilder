using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    class VSModuleImportExportManagerTest
    {
        private const string EXPECTED_IMPORT_PROJECT_NAME ="EXPECTED_IMPORT_PROJECT_NAME";
        private const string EXPECTED_IMPORT_COMPANY_SHORT_NAME ="EXPECTED_IMPORT_COMPANY_SHORT_NAME";
        private const string EXPECTED_ASSET_FOLDER = "EXPECTED_ASSET_FOLDER";
        private const string EXPECTED_PROJECT_FOLDER = "EXPECTED_PROJECT_FOLDER";
        private const string EXPECTED_REPO_LOCATION = "EXPECTED_REPO_LOCATION";
        private const string EXPECTED_COMPANY_SHORT_NAME = "EXPECTED_COMPANY_SHORT_NAME";
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_PROJECT_REPO_LOCATION = @"EXPECTED_REPO_LOCATION\EXPECTED_COMPANY_SHORT_NAME\EXPECTED_PROJECT_NAME";
        private string[] expectedExportAssetPaths = new string[]{
                                                                   @"Assets\EXPECTED_COMPANY_SHORT_NAME\EXPECTED_PROJECT_NAME",
                                                                   @"Assets\Editor\EXPECTED_COMPANY_SHORT_NAME\EXPECTED_PROJECT_NAME",
                                                                   @"Assets\ManagedCode",
                                                                   @"Assets\Editor\ManagedCode",
                                                                   @"Assets\Plugins"
                                                               };
        private const string EXPECTED_EXPORT_FILE = @"EXPECTED_REPO_LOCATION\EXPECTED_COMPANY_SHORT_NAME\EXPECTED_PROJECT_NAME\EXPECTED_PROJECT_NAME.unitypackage";
        private const string EXPECTED_SETTINGS_REPO_COPY_LOCATION = @"EXPECTED_REPO_LOCATION\EXPECTED_COMPANY_SHORT_NAME\EXPECTED_PROJECT_NAME";
        private string EXPECTED_SETTINGS_PATH = @"EXPECTED_PROJECT_FOLDER\..\UVSModule\ModuleConfig.xml";
        private string EXPECTED_IMPORT_PACKAGE_NAME = @"EXPECTED_REPO_LOCATION\EXPECTED_IMPORT_COMPANY_SHORT_NAME\EXPECTED_IMPORT_PROJECT_NAME\EXPECTED_IMPORT_PROJECT_NAME.unitypackage";

        private VSModuleImportExportManager manager;
        private UnityApi unityApi;
        private FileSystemController fsController;
        private VSModuleSettingsTO.Builder requestToBuilder;
        
        private FileEntry settingsFile;
        private VSModuleSettingsTO requestedSettingsTO;
        private bool isExported;
        private bool isImported;
        
        
        [SetUp]
        public void SetUp()
        {
            settingsFile = Substitute.For<FileEntry>();
            requestToBuilder = new VSModuleSettingsTO.Builder();
            fsController = Substitute.For<FileSystemController>();
            unityApi = Substitute.For<UnityApi>();
            manager = new VSModuleImportExportManagerImpl(unityApi, fsController);
        }

        [Test]
        public void TestExportModuleAssetsSuccess()
        {
            GivenUnityApiHasExpectedProjectFolder();
            GivenSettingsTOHasExpectedRepoLocation();
            GivenSettingsTOHasExpectedCompanyShortNameAndProjectName();
            GivenFileSystemHasSettingsFile();
            GivenFileSystemGeneratesProjectRepoLocation();
            WhenExportModuleRequested();
            ThenFileSystemProvidesSettingsFile();
            ThenMakeProjectRepoLocationRequestedOnFileSystem();
            ThenUniApiExportRequestedToProjectRepoLocationWithExpectedAssetPaths();
            ThenSettingsFileCopiedToRepoLocation();
            ThenExportIsSuccess();
        }

        private void ThenFileSystemProvidesSettingsFile()
        {
            fsController.Received().GetExistingFile(EXPECTED_SETTINGS_PATH);
        }

        private void GivenUnityApiHasExpectedProjectFolder()
        {
            unityApi.GetProjectFolder().Returns(EXPECTED_PROJECT_FOLDER);
        }

        [Test]
        public void TestExportModuleFailure()
        {
            GivenUnityApiHasExpectedProjectFolder();
            GivenSettingsTOHasExpectedRepoLocation();
            GivenFileSystemHasSettingsFile();
            GivenFileSystemFailsToGenerateRepoLocation();
            WhenExportModuleRequested();
            ThenExportIsFailure();
        }

        [Test]
        public void TestImportModuleSuccess()
        {
            GivenSettingsTOHasExpectedRepoLocation();
            GivenSettingsTOHasExpectedCompanyShortNameAndProjectName();
            GivenFileSystemHasImportPackageFile();
            WhenImportModuleRequested();
            ThenUnityImportRequestedForExpectedAssetPackage();
            ThenImportIsSuccessful();
        }
        [Test]
        public void TestImportNonExistingModuleFailure()
        {
            GivenSettingsTOHasExpectedRepoLocation();
            GivenSettingsTOHasExpectedCompanyShortNameAndProjectName();
            GivenFileSystemDoesNotHaveImportPackageFile();
            WhenImportModuleRequested();
            ThenUnityImportNotRequested();
            ThenImportIsFailure();
            
        }

        private void ThenImportIsFailure()
        {
            Assert.False(isImported);
        }

        private void ThenUnityImportNotRequested()
        {
            unityApi.DidNotReceive().ImportRootAssets(Arg.Any<String>());
        }

        private void GivenFileSystemDoesNotHaveImportPackageFile()
        {
            FileEntry nullFile = null;
            fsController.GetExistingFile(EXPECTED_IMPORT_PACKAGE_NAME).Returns(nullFile);
        }

        private void GivenFileSystemHasImportPackageFile()
        {
            FileEntry importPackageFile = Substitute.For<FileEntry>();
            importPackageFile.IsPresent().Returns(true);
            fsController.GetExistingFile(EXPECTED_IMPORT_PACKAGE_NAME).Returns(importPackageFile);
        }

        private void ThenImportIsSuccessful()
        {
            Assert.True(isImported);
        }

        private void WhenImportModuleRequested()
        {
            requestedSettingsTO = requestToBuilder.Build();
            isImported = manager.ImportModule(EXPECTED_IMPORT_COMPANY_SHORT_NAME, EXPECTED_IMPORT_PROJECT_NAME, requestedSettingsTO);
        }

        private void ThenUnityImportRequestedForExpectedAssetPackage()
        {
            unityApi.Received().ImportRootAssets(EXPECTED_IMPORT_PACKAGE_NAME);
        }

        private void ThenExportIsFailure()
        {
            Assert.False(isExported);
        }

        private void GivenFileSystemFailsToGenerateRepoLocation()
        {
            fsController.DoCreateDirectory(EXPECTED_PROJECT_REPO_LOCATION).Returns(false);

        }

        private void GivenFileSystemGeneratesProjectRepoLocation()
        {
            fsController.DoCreateDirectory(EXPECTED_PROJECT_REPO_LOCATION).Returns(true);
        }

        private void ThenExportIsSuccess()
        {
            Assert.True(isExported);
        }

        private void GivenUnityApiHasExpectedAssetsFolder()
        {
            unityApi.GetAssetFolder().Returns(EXPECTED_ASSET_FOLDER);
        }

        private void ThenSettingsFileCopiedToRepoLocation()
        {
            fsController.Received().DoFileCopy(settingsFile, EXPECTED_SETTINGS_REPO_COPY_LOCATION);
        }

        private void ThenUniApiExportRequestedToProjectRepoLocationWithExpectedAssetPaths()
        {
            unityApi.Received().ExportRootAssets(Arg.Is<String[]>(x => ValidateExportPaths(x)), EXPECTED_EXPORT_FILE);
        }

        private bool ValidateExportPaths(String[] exportPaths)
        {
            Assert.AreEqual(expectedExportAssetPaths.Count(), exportPaths.Count());
            foreach (String expectedPath in expectedExportAssetPaths)
            {
                Assert.True(exportPaths.Contains(expectedPath), "Expected but did not find export path '" + expectedPath + '\'');
            }
            return true;
        }

        private void ThenMakeProjectRepoLocationRequestedOnFileSystem()
        {
            fsController.Received().DoCreateDirectory(EXPECTED_PROJECT_REPO_LOCATION);
        }

        private void WhenExportModuleRequested()
        {
            requestedSettingsTO = requestToBuilder.Build();
            isExported = manager.ExportModule(requestedSettingsTO);
        }

        private void GivenFileSystemHasSettingsFile()
        {
            settingsFile.IsPresent().Returns(true);
            fsController.GetExistingFile(EXPECTED_SETTINGS_PATH).Returns(settingsFile);
        }

        private void GivenSettingsTOHasExpectedCompanyShortNameAndProjectName()
        {
            requestToBuilder.CompanyShortName = EXPECTED_COMPANY_SHORT_NAME;
            requestToBuilder.ProjectName = EXPECTED_PROJECT_NAME;
        }

        private void GivenSettingsTOHasExpectedRepoLocation()
        {
            requestToBuilder.RepoLocation = EXPECTED_REPO_LOCATION;
        }

    }
}
