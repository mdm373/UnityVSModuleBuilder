using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    class ProjectNameOverlayTest
    {
        private const string EXPECTED_PROJECT_LOCATION = "EXPECTED_PROJECT_LOCATION";
        private const string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const string EXPECTED_MATCH = @"[[PROJECT_NAME]]";
        private const string UNMATCHING_FILE_NAME = "UNMATCHING_FILE_NAME";
        private const string FILE_NAME_CONTAINING_NAME_EXPRESSION = "SOMEVALUE" + EXPECTED_MATCH + "SOMESUFFIX";
        private const string FILE_NAME_WITH_EXPRESSION_REPLACEMENT = "SOMEVALUE" + EXPECTED_PROJECT_NAME + "SOMESUFFIX";
        private readonly Exception EXPECTED_EXCEPTION = new Exception();

        private FileSystemController fileSystem;
        private ProjectNameOverlay overlay;
        private bool isSuccessful;
        private List<List<FileEntry>> expectedFiles;
        private FileEntry expectedFileEntry;
        private int fileRequestCounter = -1;
        private LoggingService loggingService;
        
        
        [SetUp]
        public void SetUp()
        {
            loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            this.fileSystem = Substitute.For<FileSystemController>();
            this.overlay = new ProjectNameOverlayImpl(fileSystem);
            fileRequestCounter = -1;
        }
        
        [Test]
        public void TestNameReplacementInContents()
        {
            GivenFileSystemHasFilesForLocation();
            GivenExpectedFilesWithNoNameMatch();
            WhenProjectNameOverlayRequested();
            ThenFilesHaveProjectNameExpressionReplacedWithProjectNameInContent();
            ThenFilesAreNotRenamed();
            ThenProjectNameOverlayIsSuccessful();
        }

        [Test]
        public void TestNameReplacementInFileName()
        {
            GivenFileSystemHasFilesForLocation();
            GivenExpectedFilesWithNameMatchTwiceThenNoFileWithMatch();
            WhenProjectNameOverlayRequested();
            ThenFilesHaveProjectNameExpressionReplacedWithProjectNameInContent();
            ThenExpectedFileIsRenamedWithProjectNameReplacement();
            ThenProjectNameOverlayIsSuccessful();
        }

        [Test]
        public void TestFileSystemThrowsException()
        {
            GivenFileSystemThrowsException();
            WhenProjectNameOverlayRequested();
            ThenExceptionIsLogged();
            ThenProjectNameOverlayIsNotSuccessful();
        }

        private void ThenExceptionIsLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>(), EXPECTED_EXCEPTION);
        }

        private void ThenProjectNameOverlayIsNotSuccessful()
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


        private void ThenExpectedFileIsRenamedWithProjectNameReplacement()
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


        private void ThenProjectNameOverlayIsSuccessful()
        {
            Assert.True(this.isSuccessful);
        }

        private void ThenFilesHaveProjectNameExpressionReplacedWithProjectNameInContent()
        {
            expectedFileEntry.Received().ReplaceContents(EXPECTED_MATCH, EXPECTED_PROJECT_NAME);
        }

        private void GivenFileSystemHasFilesForLocation()
        {

            fileSystem.GetFilesForLocationRecursive(EXPECTED_PROJECT_LOCATION).Returns(x => { fileRequestCounter++; return expectedFiles[fileRequestCounter].GetEnumerator(); });
        }

        private void WhenProjectNameOverlayRequested()
        {
            this.isSuccessful = overlay.Overlay(EXPECTED_PROJECT_NAME, EXPECTED_PROJECT_LOCATION);
        }
    }
}
