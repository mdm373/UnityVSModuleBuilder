using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityVSModuleBuilder;
using UnityVSModuleBuilder.FileSystem;
using UnityVSModuleBuilder.Logging;

namespace UnityVSModuleBuilder.TemplateCopy
{
    [TestFixture]
    public class TemplateCopyControllerImplTest
    {
        private const string EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";
        private const string EXPECTED_TEMPLATE_LOCATION = @"ProjectTemplate";
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
            WhenCopyRequestedForExpectedLocation();
            ThenFileSystemCopyRequestedFromTemplateLocationToExpectedLocation();
            ThenCopyRequestIsSuccssful();
        }

        [Test]
        public void TestCopyTemplateProjectFailure()
        {
            GivenFileSystemThrowsExceptionForCopy();
            WhenCopyRequestedForExpectedLocation();
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

        private void ThenCopyRequestIsSuccssful()
        {
            Assert.True(this.isSuccess);
        }

        private void WhenCopyRequestedForExpectedLocation()
        {
            isSuccess = copyController.CopyTemplate(EXPECTED_COPY_LOCATION);
        }

        private void ThenFileSystemCopyRequestedFromTemplateLocationToExpectedLocation()
        {
            fileSystem.Received().DoFullDirectoryCopy(EXPECTED_TEMPLATE_LOCATION, EXPECTED_COPY_LOCATION);
        }
    }
}
