using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Implement
{
    [TestFixture]
    class BuildProjectRequestImplTest
    {
        private const String EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const String EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";

        private BuildProjectRequestImpl request;
        
        [SetUp]
        public void SetUp()
        {
            request = new BuildProjectRequestImpl();
        }

        [Test]
        public void TestProjectName()
        {
            request.SetProjectName(EXPECTED_PROJECT_NAME);
            Assert.AreEqual(EXPECTED_PROJECT_NAME, request.GetProjectName());
        }

        [Test]
        public void TestCopyLocation()
        {
            request.SetCopyLocation(EXPECTED_COPY_LOCATION);
            Assert.AreEqual(EXPECTED_COPY_LOCATION, request.GetCopyLocation());
        }
    }
}
