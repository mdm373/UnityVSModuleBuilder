using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    public class CompanyNameOverlayTest
    {
        private const string EXPECTED_COMPANY_NAME = "EXPECTED_COMPANY_NAME";

        private CompanyNameOverlayImpl companyNameOverlay;
        private BuildProjectRequest request;
        private string value;

        [SetUp]
        public void SetUp()
        {
            this.companyNameOverlay = new CompanyNameOverlayImpl(null);
        }

        [Test]
        public void TestDefinedTag()
        {
            Assert.AreEqual("[[COMPANY_NAME]]", companyNameOverlay.GetDefinedTag());
        }

        [Test]
        public void TestOverlayValue()
        {
            GivenRequestWithCompanyName();
            WhenGettingOverlayValue();
            ThenOverlayValueIsCompanyName();
        }

        private void GivenRequestWithCompanyName()
        {
            this.request = Substitute.For<BuildProjectRequest>();
            request.GetCompanyName().Returns(EXPECTED_COMPANY_NAME);
        }

        private void WhenGettingOverlayValue()
        {
            this.value = companyNameOverlay.GetDefinedValue(request);
        }

        private void ThenOverlayValueIsCompanyName()
        {
            Assert.AreEqual(EXPECTED_COMPANY_NAME, value);
        }
    }
}
