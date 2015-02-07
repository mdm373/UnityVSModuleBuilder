using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NSubstitute;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon.XMLStore;
using UnityVSModuleCommon;

namespace UnityVSModuleCommon.Application
{
    
    [TestFixture]
    class ApplicationManagerTest
    {
        private const string REG_LOCATION = @"Software\UnityVSmoduleBuilder";
        private const string INSTALL_LOC_NAME = "install-location";
        private const string DEFAULT_REPO_LOCATION = @"APPLICATION_INSTALL_LOCATION\UVSRepo";
        private const string APPLICATION_INSTALL_LOCATION = "APPLICATION_INSTALL_LOCATION";
        private const string SETTINGS_FILE_LOCATION = @"APPLICATION_INSTALL_LOCATION\AppSettings.xml";
        private const string EXPECTED_REPO_LOCATION = "EXPECTED_REPO_LOCATION";
        private readonly Exception EXPECTED_EXCEPTION = new Exception("expected");

        private ApplicationManager applicationManager;
        private RegistryController registryController;
        private FileSystemController fsController;
        private FileEntry installDirectory;
        private FileEntry settingsFile;
        private XmlSerializerWrapper serializer;
        private ApplicationSettingsXMLModel settingsModel;
        private ApplicationSettingsResponse applicationSettingsResponse;
        private ApplicationSettings settings;
        private LoggingService loggingService;
        
        
        [SetUp]
        public void SetUp()
        {
            loggingService = Substitute.For<LoggingService>();
            settings = Substitute.For<ApplicationSettings>();
            settingsModel = new ApplicationSettingsXMLModel();
            serializer = Substitute.For<XmlSerializerWrapper>();
            fsController = Substitute.For<FileSystemController>();
            installDirectory = Substitute.For<FileEntry>();
            settingsFile = Substitute.For<FileEntry>();
            registryController = Substitute.For<RegistryController>();

            Logger.SetService(loggingService);

            applicationManager = new ApplicationManagerImpl(registryController, fsController, serializer);
        }

        [Test]
        public void TestSettingsRetrieval()
        {
            GivenRegistryHasApplicationInstallLocation();
            GivenSettingsFileExistsInInstallDirectory();
            GivenXmlSerializerHasModelForSettingsFile();
            GivenSettingsModelHasRepoLocation();
            WhenSettingsRetrievalRequested();
            ThenFileSystemProvidesSettingsFile();
            ThenXmlSerializerDeserializesSettingsFile();
            ThenResponseObjectReturned();
            ThenResponseHasSuccessCode();
            ThenSerializedModelMappedToResponseObject();
        }

        [Test]
        public void TestRetrievalNoInstallLocationFailure()
        {
            GivenRegistryHasNoApplicationInstallLocation();
            WhenSettingsRetrievalRequested();
            ThenResponseObjectReturned();
            ThenResponseObjectHasFailureNoInstallCode();
        }


        [Test]
        public void TestRetrievalForInvalidSettingsFileContents()
        {
            GivenRegistryHasApplicationInstallLocation();
            GivenSettingsFileExistsInInstallDirectory();
            GivenSerializerFailsToDeserializeFile();
            WhenSettingsRetrievalRequested();
            ThenResponseObjectReturned();
            ThenResponseHasSuccessCode();
            ThenResponseObjectHasDefaultRepoLocation();
        }

        [Test]
        public void TestRetrievalForUnknownError()
        {
            GivenRegistryRetrievalThrowsException();
            WhenSettingsRetrievalRequested();
            ThenExceptionIsLogged();
            ThenResponseObjectReturned();
            ThenResponseHasUnknownErrorCode();
        }

        private void ThenExceptionIsLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>(), EXPECTED_EXCEPTION);
        }

        private void ThenResponseHasUnknownErrorCode()
        {
            Assert.AreEqual(AppSettingsCode.UNKNOWN_ERROR, applicationSettingsResponse.GetCode());
        }

        private void GivenSerializerFailsToDeserializeFile()
        {
            ApplicationSettingsXMLModel nullModel = null;
            serializer.GetDeserialized<ApplicationSettingsXMLModel>(settingsFile).Returns(nullModel);
        }

        [Test]
        public void TestRetrievalNoSettingsFileSuccess()
        {
            GivenRegistryHasApplicationInstallLocation();
            GivenSettingsFileDoesNotExistinInInstallDirectory();
            WhenSettingsRetrievalRequested();
            ThenResponseObjectReturned();
            ThenResponseHasSuccessCode();
            ThenResponseObjectHasDefaultRepoLocation();
        }

        [Test]
        public void TestSettingsUpdate()
        {
            GivenRegistryHasApplicationInstallLocation();
            GivenSettingsHaveRepoLocation();
            GivenFileSystemHasExistingOrNewFileForSettingsFileLocation();
            WhenSettingsUpdateRequested();
            ThemXmlSerializerSerializesExpectedModel();
        }

        private void ThenResponseObjectHasFailureNoInstallCode()
        {
            Assert.AreEqual(AppSettingsCode.INSTALL_NOT_FOUND, applicationSettingsResponse.GetCode());
        }

        private void ThenResponseObjectHasDefaultRepoLocation()
        {
            Assert.AreEqual(DEFAULT_REPO_LOCATION, applicationSettingsResponse.GetApplicationSettings().GetRepoLocation());
        }

        private void ThenResponseHasSuccessCode()
        {
            Assert.AreEqual(AppSettingsCode.SUCCESS, applicationSettingsResponse.GetCode());
        }

        private void GivenFileSystemHasExistingOrNewFileForSettingsFileLocation()
        {
            fsController.GetExistingOrNewlyCreatedFile(SETTINGS_FILE_LOCATION).Returns(settingsFile);
        }

        private void GivenSettingsFileDoesNotExistinInInstallDirectory()
        {
            FileEntry nullEntry = null;
            fsController.GetExistingFile(SETTINGS_FILE_LOCATION).Returns(nullEntry);
        }

        private void ThemXmlSerializerSerializesExpectedModel()
        {
            serializer.Received().SerializeToFile<ApplicationSettingsXMLModel>(settingsFile, Arg.Is<ApplicationSettingsXMLModel>(x => ValidateSerialzedModel(x)));
        }

        private bool ValidateSerialzedModel(ApplicationSettingsXMLModel x)
        {
            Assert.AreEqual(EXPECTED_REPO_LOCATION, x.repoLocation);
            return true;
        }

        private void WhenSettingsUpdateRequested()
        {
            applicationManager.SaveApplicationSettings(settings);
        }

        private void GivenSettingsHaveRepoLocation()
        {
            settings.GetRepoLocation().Returns(EXPECTED_REPO_LOCATION);
        }

        private void ThenFileSystemProvidesSettingsFile()
        {
            fsController.Received().GetExistingFile(Arg.Is<String>(x =>  x.Equals(SETTINGS_FILE_LOCATION)));
        }

        private void ThenXmlSerializerDeserializesSettingsFile()
        {
            serializer.Received().GetDeserialized<ApplicationSettingsXMLModel>(settingsFile);
        }

        private void ThenSerializedModelMappedToResponseObject()
        {
            ApplicationSettings settings = applicationSettingsResponse.GetApplicationSettings();
            Assert.AreEqual(EXPECTED_REPO_LOCATION, settings.GetRepoLocation());
        }

        private void ThenResponseObjectReturned()
        {
            Assert.NotNull(applicationSettingsResponse);
        }

        private void WhenSettingsRetrievalRequested()
        {
            applicationSettingsResponse =applicationManager.RetrieveApplicationSettings();
        }

        private void GivenSettingsModelHasRepoLocation()
        {
            settingsModel.repoLocation = EXPECTED_REPO_LOCATION;
        }

        private void GivenXmlSerializerHasModelForSettingsFile()
        {
            serializer.GetDeserialized<ApplicationSettingsXMLModel>(settingsFile).Returns(settingsModel);
        }

        private void GivenSettingsFileExistsInInstallDirectory()
        {
            fsController.GetExistingFile(SETTINGS_FILE_LOCATION).Returns(settingsFile);
        }

        private void GivenFileSystemHasApplicationInstallDirectory()
        {
            fsController.GetExistingFile(APPLICATION_INSTALL_LOCATION).Returns(installDirectory);
        }

        private void GivenRegistryRetrievalThrowsException()
        {
            registryController.When(x => x.GetRegistryKey<String>(RegKeyType.HK_LOCAL_MACHINE, REG_LOCATION, INSTALL_LOC_NAME)).Do(x => { throw EXPECTED_EXCEPTION; });
        }

        private void GivenRegistryHasApplicationInstallLocation()
        {
            registryController.GetRegistryKey<String>(RegKeyType.HK_LOCAL_MACHINE, REG_LOCATION, INSTALL_LOC_NAME).Returns(APPLICATION_INSTALL_LOCATION);
        }

        private void GivenRegistryHasNoApplicationInstallLocation()
        {
            String nullValue = null;
            registryController.GetRegistryKey<String>(RegKeyType.HK_LOCAL_MACHINE, REG_LOCATION, INSTALL_LOC_NAME).Returns(nullValue);
        }
    }
}
