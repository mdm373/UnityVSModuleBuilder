using NSubstitute;
using NUnit.Framework;
using UnityVSModuleBuilder.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.TemplateCopy;


namespace UnityVSModuleBuilder.Implement
{
    [TestFixture]
    class TemplateProjectBuilderTest
    {
        private const String EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const String EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";

        private BuildProjectRequest request;
        private TemplateProjectBuilderImpl templateProjectBuilder;
        private BuildProjectResponse response;
        private ProjectNameOverlay nameOverlay;
        private TemplateCopyController templateCopy;
        
        

        [SetUp]
        public void SetUp()
        {
            this.nameOverlay = Substitute.For<ProjectNameOverlay>();
            this.templateCopy = Substitute.For<TemplateCopyController>();
            this.templateProjectBuilder = TemplateProjectBuilderImpl.GetInstance(templateCopy, nameOverlay);
            GivenBuildProjectRequest();
            GivenBuildProjectRequestHasExpectedName();
            GivenBuildProjectRequestHasExpectedCopyLocation();
        }

        [Test]
        public void TestProjectBuildSuccess()
        {
            
            GivenTemplateCopySuccessful();
            GivenNameOverlayIsSuccessful();
            WhenBuildProjectRequested();
            ThenProjectNameOverlayRequestedWithExpectedNameAndLocation();
            ThenTemplateCopyRequestedWithExpectedLocation();
            ThenBuildProjectResponseIsSuccessful();
        }

        private void ThenTemplateCopyRequestedWithExpectedLocation()
        {
            this.templateCopy.Received().CopyTemplate(EXPECTED_COPY_LOCATION);
        }

        private void GivenTemplateCopySuccessful()
        {
            this.templateCopy.CopyTemplate(Arg.Any<String>()).Returns(true);
        }

        private void GivenTemplateCopyFailure()
        {
            this.templateCopy.CopyTemplate(Arg.Any<String>()).Returns(false);
        }

        [Test]
        public void TestProjectBuildNameOverlayFailure()
        {
            GivenNameOverlayIsFailure();
            WhenBuildProjectRequested();
            ThenBuildProjectResponseIsFailure();
        }

        [Test]
        public void TestProjectBuildTemplateCopyFailure()
        {
            GivenTemplateCopyFailure();
            WhenBuildProjectRequested();
            ThenProjectNameOverlayNotRequested();
            ThenBuildProjectResponseIsFailure();

        }

        private void ThenProjectNameOverlayNotRequested()
        {
            nameOverlay.DidNotReceive().Overlay(Arg.Any<String>(), Arg.Any<String>());
        }

        private void GivenNameOverlayIsFailure()
        {
            nameOverlay.Overlay(Arg.Any<String>(), Arg.Any<String>()).Returns(false);
        }

        private void ThenBuildProjectResponseIsFailure()
        {
            Assert.False(response.IsSuccess());
        }

        private void ThenBuildProjectResponseIsSuccessful()
        {
            Assert.True(response.IsSuccess());
        }

        private void GivenNameOverlayIsSuccessful()
        {
            nameOverlay.Overlay(Arg.Any<String>(), Arg.Any<String>()).Returns(true);
        }

        private void ThenProjectNameOverlayRequestedWithExpectedNameAndLocation()
        {
            nameOverlay.Received().Overlay(EXPECTED_PROJECT_NAME, EXPECTED_COPY_LOCATION);
        }

        private void GivenBuildProjectRequestHasExpectedName()
        {
            this.request.GetProjectName().Returns(EXPECTED_PROJECT_NAME);
        }

        private void WhenBuildProjectRequested()
        {
            this.response = templateProjectBuilder.DoBuild(request);
        }

        private void GivenBuildProjectRequest()
        {
            this.request = Substitute.For<BuildProjectRequest>();
        }

        private void GivenBuildProjectRequestHasExpectedCopyLocation()
        {
            this.request.GetCopyLocation().Returns(EXPECTED_COPY_LOCATION);
        }
    }
}
