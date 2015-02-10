using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityVSModuleCommon.FileSystem;
using UnityVSModuleCommon;

namespace UnityVSModuleBuilder.TemplateCopy
{
    [TestFixture]
    public class TemplateCopyControllerImplTest
    {
        private const string EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";
        private string EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private string EXPECTED_COPY_FULL_LOCATION = @"EXPECTED_COPY_LOCATION\EXPECTED_PROJECT_NAME";
        private const string EXPECTED_REM_TAG_FILE_NAME = "[[REM_TAG]]";
        private const string EXPECTED_REM_TAG_FILE_PATH = "EXPECTED_REM_TAG_FILE_PATH";
        private const string EXPECTED_TEMPLATE_LOCATION = @"ProjectTemplate";
        private const string EXPECTED_NON_REM_TAG_FILE_NAME = "EXPECTED_NON_REM_TAG_FILE_NAME";
        private const string EXPECTED_NON_REM_TAG_FILE_PATH = "EXPECTED_NON_REM_TAG_FILE_PATH";
        private readonly Exception EXPECTED_FS_EXCEPTION = new Exception();

        private TemplateCopyController copyController;
        private Boolean isSuccess;
        private FileSystemController fileSystem;
        private LoggingService loggingService;
        
        
        
        
        
        [SetUp]
        public void SetUp()
        {
            this.loggingService = Substitute.For<LoggingService>();
            Logger.SetService(loggingService);
            this.fileSystem = Substitute.For<FileSystemController>();
            
            this.copyController = new TemplateCopyControllerImpl(fileSystem);
        }

        [Test]
        public void TestCopyTemplateProject()
        {
            GivenFileSystemContainsRemTagFileAndNonRemTagFile();
            WhenCopyRequestedForExpectedLocationAndName();
            ThenFileSystemCopyRequestedFromTemplateLocationToExpectedLocation();
            ThenCopyRequestIsSuccssful();
            ThenRemTagItemsAreDeleted();
            ThenNonRemTagItemsAreNotDeleted();
        }

        private void GivenFileSystemContainsRemTagFileAndNonRemTagFile()
        {
            List<FileEntry> expectedFiles = new List<FileEntry>();
            FileEntry remTagFile = GetSubstitueFileEntry(EXPECTED_REM_TAG_FILE_NAME, EXPECTED_REM_TAG_FILE_PATH);
            FileEntry nonRemTagFile = GetSubstitueFileEntry(EXPECTED_NON_REM_TAG_FILE_NAME, EXPECTED_NON_REM_TAG_FILE_PATH);
            expectedFiles.Add(remTagFile);
            expectedFiles.Add(nonRemTagFile);
            fileSystem.GetFilesForLocationRecursive(EXPECTED_COPY_FULL_LOCATION).Returns(x => { return expectedFiles.GetEnumerator(); });
        }

        private static FileEntry GetSubstitueFileEntry(String fileName, String filePath)
        {
            FileEntry remTagFile = Substitute.For<FileEntry>();
            remTagFile.GetFileType().Returns(FileType.FILE);
            remTagFile.GetFileName().Returns(fileName);
            remTagFile.GetFilePath().Returns(filePath);
            return remTagFile;
        }

        

        [Test]
        public void TestCopyTemplateProjectFailure()
        {
            GivenFileSystemThrowsExceptionForCopy();
            WhenCopyRequestedForExpectedLocationAndName();
            ThenCopyRequestIsFailure();
            ThenFailureIsLogged();
        }

        private void ThenFailureIsLogged()
        {
            loggingService.Received().LogError(Arg.Any<String>(), EXPECTED_FS_EXCEPTION);
        }

        private void ThenCopyRequestIsFailure()
        {
            Assert.False(this.isSuccess);
        }

        private void GivenFileSystemThrowsExceptionForCopy()
        {
            fileSystem.When(x => x.DoFullDirectoryCopy(Arg.Any<String>(), Arg.Any<String>())).Do(x => { throw EXPECTED_FS_EXCEPTION; });
        }

        private void ThenNonRemTagItemsAreNotDeleted()
        {
            fileSystem.DidNotReceive().DeleteFile(EXPECTED_NON_REM_TAG_FILE_PATH);
        }

        private void ThenRemTagItemsAreDeleted()
        {
            fileSystem.Received().DeleteFile(EXPECTED_REM_TAG_FILE_PATH);
        }

        private void ThenCopyRequestIsSuccssful()
        {
            Assert.True(this.isSuccess);
        }

        private void WhenCopyRequestedForExpectedLocationAndName()
        {
            isSuccess = copyController.CopyAndCleanTemplate(EXPECTED_COPY_LOCATION, EXPECTED_PROJECT_NAME);
        }

        private void ThenFileSystemCopyRequestedFromTemplateLocationToExpectedLocation()
        {
            fileSystem.Received().DoFullDirectoryCopy(EXPECTED_TEMPLATE_LOCATION, EXPECTED_COPY_FULL_LOCATION);
        }
    }
}
