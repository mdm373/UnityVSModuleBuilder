using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    public class EditorManagedCodeOverlayTest
    {
        private const string EDITOR_DLL_FILE_LOCATION = "UnityVSModuleEditor.dll";
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";
        private const string EXPECTED_UNITY_EDITOR_ASSET_LOCATION = @"EXPECTED_COPY_LOCATION\EXPECTED_PROJECT_NAME\UnityGame\Assets\Editor";

        private  DefinedOverlay overlay;
        private FileSystemController fileSystem;
        private FileEntry expectedEditorDLLFile;
        private BuildProjectRequest request;
        private bool isSuccessful;
        private LoggingService loggingService;
        

        [SetUp]
        public void SetUp(){
            this.loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            this.fileSystem = Substitute.For<FileSystemController>();
            this.overlay = new EditorManagedCodeOverlay(fileSystem);
            this.expectedEditorDLLFile = Substitute.For<FileEntry>();
            this.request = Substitute.For<BuildProjectRequest>();
        }

        [Test]
        public void TestEditorDLLCopiedToTemplateLocation()
        {
            GivenFileSystemHasEditorDLL();
            GivenRequestHasProjectNameAndCopyLocation();
            WhenOverlayRequested();
            ThenDLLCopiedToUnityEditorAssetDirectory();
            ThenOverlaySuccessful();
        }

        private void ThenOverlaySuccessful()
        {
            Assert.True(isSuccessful);
        }

        [Test]
        public void TestEditorDLLDoesNotExist() 
        {
            GivenFileSystemHasNoEditorDLL();
            WhenOverlayRequested();
            ThenIssueIsLogged();
            ThenOverlayIsFailure();
        }

        private void ThenOverlayIsFailure()
        {
            Assert.False(isSuccessful);
        }

        private void ThenIssueIsLogged()
        {
            loggingService.Received().Log(Arg.Any<String>());
        }

        private void GivenFileSystemHasNoEditorDLL()
        {
            FileEntry result = null;
            fileSystem.GetFile(EDITOR_DLL_FILE_LOCATION).Returns(result);
        }

        private void ThenDLLCopiedToUnityEditorAssetDirectory()
        {
            fileSystem.Received().DoFileCopy(expectedEditorDLLFile, EXPECTED_UNITY_EDITOR_ASSET_LOCATION);
        }

        private void GivenRequestHasProjectNameAndCopyLocation()
        {
            request.GetProjectName().Returns(EXPECTED_PROJECT_NAME);
            request.GetCopyLocation().Returns(EXPECTED_COPY_LOCATION);
        }

        private void GivenFileSystemHasEditorDLL()
        {
            fileSystem.GetFile(EDITOR_DLL_FILE_LOCATION).Returns(expectedEditorDLLFile);
        }

        private void WhenOverlayRequested()
        {
            this.isSuccessful = overlay.Overlay(request);
        }
    }
}
