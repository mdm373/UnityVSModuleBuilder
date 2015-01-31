using NSubstitute;
using NUnit.Framework;
using System;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    class ProjectNameOverlayTest
    {
        private const String EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";

        private FileSystemController fileSystem;
        private ProjectNameOverlay projectNameOverlay;
        private BuildProjectRequest request;
        private string value;

        [SetUp]
        public void SetUp()
        {
            this.fileSystem = Substitute.For<FileSystemController>();
            this.projectNameOverlay = new ProjectNameOverlay(fileSystem);
        }

        [Test]
        public void TestProjectNameDefinedTag()
        {
            Assert.AreEqual("[[PROJECT_NAME]]", projectNameOverlay.GetDefinedTag());
        }

        [Test]
        public void TestProjectNameOverlayValue()
        {
            GivenRequestWithProjectName();
            WhenGettingValueFromRequest();
            ThenValueIsProjectName();
        }

        private void ThenValueIsProjectName()
        {
            Assert.AreEqual(EXPECTED_PROJECT_NAME, value);
        }

        private void WhenGettingValueFromRequest()
        {
            this.value = this.projectNameOverlay.GetDefinedValue(request);
        }

        private void GivenRequestWithProjectName()
        {
            this.request = Substitute.For<BuildProjectRequest>();
            request.GetProjectName().Returns(EXPECTED_PROJECT_NAME);
        }
    }
}
