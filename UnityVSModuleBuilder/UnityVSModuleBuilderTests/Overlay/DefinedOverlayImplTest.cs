using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    class DefinedOverlayImplTest
    {
        private const string EXPECTED_PROJECT_LOCATION = "EXPECTED_PROJECT_LOCATION";
        private const string EXPECTED_DEFINED_VALUE = "EXPECTED_DEFINED_VALUE";
        private const string EXPECTED_MATCH = @"[[DEFINED_NAME]]";
        private const string UNMATCHING_FILE_NAME = "UNMATCHING_FILE_NAME";
        private const string FILE_NAME_CONTAINING_NAME_EXPRESSION = "SOMEVALUE" + EXPECTED_MATCH + "SOMESUFFIX";
        private const string FILE_NAME_WITH_EXPRESSION_REPLACEMENT = "SOMEVALUE" + EXPECTED_DEFINED_VALUE + "SOMESUFFIX";
        private readonly Exception EXPECTED_EXCEPTION = new Exception();

        private FileSystemController fileSystem;
        private DefinedOverlay overlay;
        private bool isSuccessful;
        private List<List<FileEntry>> expectedFiles;
        private FileEntry expectedFileEntry;
        private int fileRequestCounter = -1;
        private LoggingService loggingService;


        private class MockDefinedOverlay : DefinedOverlayImpl
        {
            public MockDefinedOverlay(FileSystemController fileSystem) : base(fileSystem) {  }
            public override string GetDefinedValue(BuildProjectRequest request)
            {
                return EXPECTED_DEFINED_VALUE;
            }

            public override string GetDefinedTag()
            {
                return EXPECTED_MATCH;
            }
        }

        [SetUp]
        public void SetUp()
        {
            loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            this.fileSystem = Substitute.For<FileSystemController>();
            this.overlay = new MockDefinedOverlay(fileSystem);
            fileRequestCounter = -1;
        }
        
        [Test]
        public void TestNameReplacementInContents()
        {
            GivenFileSystemHasFilesForLocation();
            GivenExpectedFilesWithNoNameMatch();
            WhenOverlayRequested();
            ThenFilesHaveDefinedTagReplacedWithDefinedValueInContent();
            ThenFilesAreNotRenamed();
            ThenOverlayIsSuccessful();
        }

        [Test]
        public void TestNameReplacementInFileName()
        {
            GivenFileSystemHasFilesForLocation();
            GivenExpectedFilesWithNameMatchTwiceThenNoFileWithMatch();
            WhenOverlayRequested();
            ThenFilesHaveDefinedTagReplacedWithDefinedValueInContent();
            ThenExpectedFileIsRenamedWithDefinedTagReplacement();
            ThenOverlayIsSuccessful();
        }

        [Test]
        public void TestFileSystemThrowsException()
        {
            GivenFileSystemThrowsException();
            WhenOverlayRequested();
            ThenExceptionIsLogged();
            ThenOverlayIsNotSuccessful();
        }

        private void ThenExceptionIsLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>(), EXPECTED_EXCEPTION);
        }

        private void ThenOverlayIsNotSuccessful()
        {
            Assert.False(isSuccessful);
        }

        private void GivenFileSystemThrowsException()
        {
            fileSystem.GetFilesForLocationRecursive(EXPECTED_PROJECT_LOCATION).Returns(x => { throw EXPECTED_EXCEPTION; });
        }

        private void ThenFilesAreNotRenamed()
        {
            expectedFileEntry.DidNotReceive().RenameFile(Arg.Any<String>());
        }

        private void GivenExpectedFilesWithNameMatchTwiceThenNoFileWithMatch()
        {
            expectedFileEntry = Substitute.For<FileEntry>();
            expectedFileEntry.GetFileType().Returns(FileType.FILE);
            expectedFileEntry.GetFileName().Returns(FILE_NAME_CONTAINING_NAME_EXPRESSION);
            expectedFiles = new List<List<FileEntry>>();
            List<FileEntry> expectedFileListWithName = new List<FileEntry>();
            List<FileEntry> expectedFileListWithoutName = new List<FileEntry>();
            expectedFileListWithName.Add(expectedFileEntry);
            expectedFiles.Add(expectedFileListWithName);
            expectedFiles.Add(expectedFileListWithName);
            expectedFiles.Add(expectedFileListWithoutName);
        }


        private void ThenExpectedFileIsRenamedWithDefinedTagReplacement()
        {
            expectedFileEntry.Received().RenameFile(FILE_NAME_WITH_EXPRESSION_REPLACEMENT);
        }

        private void GivenExpectedFilesWithNoNameMatch()
        {
            expectedFileEntry = Substitute.For<FileEntry>();
            expectedFileEntry.GetFileType().Returns(FileType.FILE);
            expectedFileEntry.GetFileName().Returns(UNMATCHING_FILE_NAME);
            expectedFiles = new List<List<FileEntry>>();
            List<FileEntry> expectedFileListWithName = new List<FileEntry>();
            expectedFileListWithName.Add(expectedFileEntry);
            expectedFiles.Add(expectedFileListWithName);
            expectedFiles.Add(expectedFileListWithName);
        }


        private void ThenOverlayIsSuccessful()
        {
            Assert.True(this.isSuccessful);
        }

        private void ThenFilesHaveDefinedTagReplacedWithDefinedValueInContent()
        {
            expectedFileEntry.Received().ReplaceContents(EXPECTED_MATCH, EXPECTED_DEFINED_VALUE);
        }

        private void GivenFileSystemHasFilesForLocation()
        {

            fileSystem.GetFilesForLocationRecursive(EXPECTED_PROJECT_LOCATION).Returns(x => { fileRequestCounter++; return expectedFiles[fileRequestCounter].GetEnumerator(); });
        }

        private void WhenOverlayRequested()
        {
            BuildProjectRequest request = Substitute.For<BuildProjectRequest>();
            request.GetCopyLocation().Returns(EXPECTED_PROJECT_LOCATION);
            this.isSuccessful = overlay.Overlay(request);
        }
    }
}
