using NSubstitute;
using NUnit.Framework;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    class CompanyShortNameOverlayTest
    {
        private readonly DefinedOverlayTestHelper testHelper = new DefinedOverlayTestHelper();

        [SetUp]
        public void SetUp()
        {
            testHelper.SetUp(new CompanyShortNameOverlay(null));
        }

        [Test]
        public void TestCompanyShortNameDefinedTag()
        {
            testHelper.TestDefinedTag("[[COMPANY_SHORT_NAME]]");
        }

        [Test]
        public void TestCompanyShortNameOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetCompanyShortName().Returns(expectedRequestValue); return true; });
        }
    }
}
