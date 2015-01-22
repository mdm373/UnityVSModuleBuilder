using NSubstitute;
using NUnit.Framework;
using UnityVSModuleBuilder.Overlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.Implement;


namespace UnityVSModuleBuilder
{
    [TestFixture]
    class TemplateProjectBuilderTest
    {
        private const String EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";

        private BuildProjectRequest request;
        private TemplateProjectBuilderImpl templateProjectBuilder;
        private BuildProjectResponse response;
        private ProjectNameOverlay nameOverlay;
        

        [SetUp]
        public void SetUp()
        {
            this.nameOverlay = Substitute.For<ProjectNameOverlay>();
            this.templateProjectBuilder = TemplateProjectBuilderImpl.GetInstance(nameOverlay);
            GivenBuildProjectRequest();
            GivenBuildProjectRequestHasExpectedName();
        }

        [Test]
        public void TestProjectBuildSuccess()
        {
            GivenNameOverlayIsSuccessful();
            WhenBuildProjectRequested();
            ThenProjectNameOverlayRequestedWithExpectedName();
            ThenBuildProjectResponseIsSuccessful();
        }

        [Test]
        public void TestProjectBuildNameOverlayFailure()
        {
            GivenNameOverlayIsFailure();
            WhenBuildProjectRequested();
            ThenBuildProjectResponseIsFailure();
        }

        private void GivenNameOverlayIsFailure()
        {
            nameOverlay.Overlay(Arg.Any<String>()).Returns(false);
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
            nameOverlay.Overlay(Arg.Any<String>()).Returns(true);
        }

        private void ThenProjectNameOverlayRequestedWithExpectedName()
        {
            nameOverlay.Received().Overlay(EXPECTED_PROJECT_NAME);
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
    }
}
