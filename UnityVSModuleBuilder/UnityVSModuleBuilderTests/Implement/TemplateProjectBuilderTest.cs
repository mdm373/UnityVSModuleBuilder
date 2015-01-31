using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityVSModuleBuilder.Overlay;
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
        private TemplateCopyController templateCopy;
        private List<DefinedOverlay> overlays;
        private DefinedOverlay firstOverlay;
        
        

        [SetUp]
        public void SetUp()
        {
            this.overlays = new List<DefinedOverlay>();
            this.firstOverlay = Substitute.For<DefinedOverlay>();
            this.overlays.Add(firstOverlay);
            this.templateCopy = Substitute.For<TemplateCopyController>();
            this.templateProjectBuilder = TemplateProjectBuilderImpl.GetInstance(templateCopy, overlays);
            GivenBuildProjectRequest();
            GivenBuildProjectRequestHasExpectedName();
            GivenBuildProjectRequestHasExpectedCopyLocation();
        }

        [Test]
        public void TestProjectBuildSuccess()
        {
            
            GivenTemplateCopySuccessful();
            GivenOverlayIsSuccessful();
            WhenBuildProjectRequested();
            ThenOverlayRequestedWithExpectedRequest();
            ThenTemplateCopyRequestedWithExpectedLocation();
            ThenBuildProjectResponseIsSuccessful();
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
            ThenOverlayNotRequested();
            ThenBuildProjectResponseIsFailure();

        }

        private void ThenTemplateCopyRequestedWithExpectedLocation()
        {
            this.templateCopy.Received().CopyAndCleanTemplate(EXPECTED_COPY_LOCATION);
        }

        private void GivenTemplateCopySuccessful()
        {
            this.templateCopy.CopyAndCleanTemplate(Arg.Any<String>()).Returns(true);
        }

        private void GivenTemplateCopyFailure()
        {
            this.templateCopy.CopyAndCleanTemplate(Arg.Any<String>()).Returns(false);
        }

        private void ThenOverlayNotRequested()
        {
            firstOverlay.DidNotReceive().Overlay(Arg.Any<BuildProjectRequest>());
        }

        private void GivenNameOverlayIsFailure()
        {
            firstOverlay.Overlay(Arg.Any<BuildProjectRequest>()).Returns(false);
        }

        private void ThenBuildProjectResponseIsFailure()
        {
            Assert.False(response.IsSuccess());
        }

        private void ThenBuildProjectResponseIsSuccessful()
        {
            Assert.True(response.IsSuccess());
        }

        private void GivenOverlayIsSuccessful()
        {
            firstOverlay.Overlay(Arg.Any<BuildProjectRequest>()).Returns(true);
        }

        private void ThenOverlayRequestedWithExpectedRequest()
        {
            firstOverlay.Received().Overlay(this.request);
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
