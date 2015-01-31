using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon.Logging;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    [TestFixture]
    public class VSModuleDependencyManagerTest
    {
        private const string EXPECTED_ASSET_FOLDER = "EXPECTED_ASSET_FOLDER";
        private const string EXPECTED_DEPENDENCY_FILE_LOCATION = @"EXPECTED_ASSET_FOLDER\Editor\ModuleDependencies.xml";
        private const string EXPECTED_EXISTING_COMPANY_SHORT_NAME = "EXPECTED_COMPANY_SHORT_NAME";
        private const string EXPECTED_EXISTING_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_ADDED_COMPANY_SHORT_NAME = "EXPECTED_ADDED_COMPANY_SHORT_NAME";
        private const string EXPECTED_ADDED_PROJECT_NAME = "EXPECTED_ADDED_PROJECT_NAME";

        private VSModuleDependencyManager manager;
        private FileSystemController fsController;
        private XmlSerializerWrapper serializer;
        private UnityApi unityApi;
        private VSModuleDependencyTO retrievedTO;
        private FileEntry dependencyFile;
        private VSModuleDependencyXmlModel deserializedModel;
        private DependencyItem dependencyItem;
        private bool isAdded;
        private LoggingService loggingService;
        
        
        
        [SetUp]
        public void SetUp()
        {
            loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            deserializedModel = new VSModuleDependencyXmlModel();
            dependencyFile = Substitute.For<FileEntry>();
            unityApi = Substitute.For<UnityApi>();
            serializer = Substitute.For<XmlSerializerWrapper>();
            fsController = Substitute.For<FileSystemController>();
            this.manager = new VSModuleDependencyManagerImpl(unityApi, serializer, fsController);
        }

        [Test]
        public void TestDependencyRetrievedSuccessfully()
        {
            GivenUnityAPIHasExpectedAssetFolder();
            GivenDependencyFileExists();
            GivenSerializerHasModelForFile();
            GivenModelHasExpectedDependencyItems();
            WhenManagerRetrievesDependencyTO();
            ThenRetrievedTOHasExpectedDependencyItems();
        }

        [Test]
        public void TestDependencyRetrieveForNonExistingDependencyFile()
        {
            GivenUnityAPIHasExpectedAssetFolder();
            GivenDependencyFileDoesNotExist();
            WhenManagerRetrievesDependencyTO();
            ThenRetrievedTOHasNoDependencies();
        }

        [Test]
        public void TestDependencyAddForNewDependency()
        {
            GivenUnityAPIHasExpectedAssetFolder();
            GivenDependencyFileExists();
            GivenSerializerHasModelForFile();
            GivenModelHasExpectedDependencyItems();
            WhenDependencyAddRequested();
            ThenModelSerializedWithAddedItems();
            ThenIsAddIsSuccessful();
        }

        [Test]
        public void TestDuplicateDependencyAdded()
        {
            GivenUnityAPIHasExpectedAssetFolder();
            GivenDependencyFileExists();
            GivenSerializerHasModelForFile();
            GivenModelHasExpectedDependencyItems();
            WhenDuplicateDependencyRquested();
            ThenSerializerUpdateIsNotRequested();
            ThenAddedIsFalse();
            ThenErrorIsLogged();
        }

        private void ThenErrorIsLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>());
        }

        private void ThenAddedIsFalse()
        {
            Assert.False(isAdded);
        }

        private void ThenSerializerUpdateIsNotRequested()
        {
            serializer.DidNotReceive().SerializeToFile<VSModuleDependencyXmlModel>(Arg.Any<FileEntry>(), Arg.Any<VSModuleDependencyXmlModel>());
        }

        private void WhenDuplicateDependencyRquested()
        {
            isAdded = manager.AddDependency(EXPECTED_EXISTING_COMPANY_SHORT_NAME, EXPECTED_EXISTING_PROJECT_NAME);
        }
        

        private void ThenIsAddIsSuccessful()
        {
            Assert.True(isAdded);
        }

        private void WhenDependencyAddRequested()
        {
            isAdded = this.manager.AddDependency(EXPECTED_ADDED_COMPANY_SHORT_NAME, EXPECTED_ADDED_PROJECT_NAME);
        }

        private void ThenModelSerializedWithAddedItems()
        {
            serializer.Received().SerializeToFile<VSModuleDependencyXmlModel>(Arg.Is<FileEntry>(dependencyFile), Arg.Is<VSModuleDependencyXmlModel>( x => ValidateNewDependencyAdded(x)));
        }

        private static bool ValidateNewDependencyAdded(VSModuleDependencyXmlModel x)
        {
            Assert.AreEqual(2, x.dependencies.Count);
            Assert.AreEqual(EXPECTED_ADDED_COMPANY_SHORT_NAME, x.dependencies[1].companyShortName);
            Assert.AreEqual(EXPECTED_ADDED_PROJECT_NAME, x.dependencies[1].projectName);
            return true;
        }

        

        private void ThenRetrievedTOHasNoDependencies()
        {
            Assert.AreEqual(0, retrievedTO.GetDependencyCount());
        }

        private void GivenDependencyFileDoesNotExist()
        {
            FileEntry nullFile = null;
            fsController.GetExistingFile(EXPECTED_DEPENDENCY_FILE_LOCATION).Returns(nullFile);
        }

        private void ThenRetrievedTOHasExpectedDependencyItems()
        {
            Assert.AreEqual(1, retrievedTO.GetDependencyCount());
            List<VSModuleDependencyItem>.Enumerator dependencies = retrievedTO.GetDependencies();
            dependencies.MoveNext();
            Assert.AreEqual(EXPECTED_EXISTING_COMPANY_SHORT_NAME, dependencies.Current.GetCompanyShortName());
            Assert.AreEqual(EXPECTED_EXISTING_PROJECT_NAME, dependencies.Current.GetProjectName());
        }

        private void GivenModelHasExpectedDependencyItems()
        {
            dependencyItem = new DependencyItem();
            dependencyItem.companyShortName = EXPECTED_EXISTING_COMPANY_SHORT_NAME;
            dependencyItem.projectName = EXPECTED_EXISTING_PROJECT_NAME;
            deserializedModel.dependencies = new List<DependencyItem>();
            deserializedModel.dependencies.Add(dependencyItem);
        }

        private void GivenSerializerHasModelForFile()
        {
            serializer.GetDeserialized<VSModuleDependencyXmlModel>(dependencyFile).Returns(deserializedModel);
        }

        private void GivenDependencyFileExists()
        {
            fsController.GetExistingFile(EXPECTED_DEPENDENCY_FILE_LOCATION).Returns(dependencyFile);
            fsController.GetExistingOrNewlyCreatedFile(EXPECTED_DEPENDENCY_FILE_LOCATION).Returns(dependencyFile);
            dependencyFile.IsPresent().Returns(true);
        }

        private void GivenUnityAPIHasExpectedAssetFolder()
        {
            unityApi.GetAssetFolder().Returns(EXPECTED_ASSET_FOLDER);
        }

        private void WhenManagerRetrievesDependencyTO()
        {
            retrievedTO = manager.GetDependencyTO();
        }
    }
}
