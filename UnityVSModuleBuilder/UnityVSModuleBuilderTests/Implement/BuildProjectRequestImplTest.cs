using NUnit.Framework;
using System;

namespace UnityVSModuleBuilder.Implement
{
    [TestFixture]
    class BuildProjectRequestImplTest
    {
        private const String EXPECTED_PROJECT_NAME = "EXPECTED_PROJECT_NAME";
        private const String EXPECTED_COPY_LOCATION = "EXPECTED_COPY_LOCATION";
        private const String EXPECTED_UNITY_LOCATION = "EXPECTED_UNITY_LOCATION";
        private const String EXPECTED_COMPANY_SHORT_NAME = "EXPECTED_COMPANY_SHORT_NAME";
        private const String EXPECTED_COMPANY_NAME = "EXPECTED_COMPANY_NAME";
        private const String EXPECTED_MODULE_REPOSITORY_LOCATION = "EXPECTED_MODULE_REPOSITORY_LOCATION";

        private BuildProjectRequest request;
        private BuildProjectRequestImpl.Builder builder;
        
        
        [SetUp]
        public void SetUp()
        {
            builder = new BuildProjectRequestImpl.Builder();
        }

        [Test]
        public void TestValidBuildRequest()
        {
            GivenBuilderHasExpectedCopyLocation();
            GivenBuilderHasExpectedCompanyName();
            GivenBuilderHasExpectedCompanyShortName();
            GivenBuilderHasExpectedUnityLocation();
            GivenBuilderHasExpectedProjectName();
            GivenBuilderHasModuleRepositoryLocation();
            WhenBuildInvoked();
            ThenRequestHasModuleRepositoryLocation();
            ThenRequestHasExpectedCopyLocation();
            ThenRequestHasExpectedCompanyName();
            ThenRequestHasExpectedCompanyShortName();
            ThenRequestHasExpectedUnityLocation();
            ThenRequestHasExpectedProjectName();
        }

        private void ThenRequestHasModuleRepositoryLocation()
        {
            Assert.AreEqual(EXPECTED_MODULE_REPOSITORY_LOCATION, request.GetModuleRepositoryLocation());
        }

        private void GivenBuilderHasModuleRepositoryLocation()
        {
            builder.moduleRepositoryLocation = EXPECTED_MODULE_REPOSITORY_LOCATION;
        }

        private void ThenRequestHasExpectedProjectName()
        {
            Assert.AreEqual(EXPECTED_PROJECT_NAME, request.GetProjectName());
        }

        private void ThenRequestHasExpectedUnityLocation()
        {
            Assert.AreEqual(EXPECTED_UNITY_LOCATION, request.GetUnityLocation());
        }

        private void ThenRequestHasExpectedCompanyShortName()
        {
            Assert.AreEqual(EXPECTED_COMPANY_SHORT_NAME, request.GetCompanyShortName());
        }

        private void ThenRequestHasExpectedCompanyName()
        {
            Assert.AreEqual(EXPECTED_COMPANY_NAME, request.GetCompanyName());
        }

        private void GivenBuilderHasExpectedProjectName()
        {
            builder.projectName = EXPECTED_PROJECT_NAME;
        }

        private void GivenBuilderHasExpectedUnityLocation()
        {
            builder.unityLocation = EXPECTED_UNITY_LOCATION;
        }

        private void GivenBuilderHasExpectedCompanyShortName()
        {
            builder.companyShortName = EXPECTED_COMPANY_SHORT_NAME;
        }

        private void GivenBuilderHasExpectedCompanyName()
        {
            builder.companyName = EXPECTED_COMPANY_NAME;
        }

        private void ThenRequestHasExpectedCopyLocation()
        {
            Assert.AreEqual(EXPECTED_COPY_LOCATION, request.GetCopyLocation());
        }

        private void WhenBuildInvoked()
        {
            request = builder.Build();
        }

        private void GivenBuilderHasExpectedCopyLocation()
        {
            builder.copyLocation = EXPECTED_COPY_LOCATION;
        }

    }
}
