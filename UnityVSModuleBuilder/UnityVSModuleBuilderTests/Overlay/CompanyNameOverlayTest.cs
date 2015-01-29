using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    public class CompanyNameOverlayTest
    {
        private readonly DefinedOverlayTestHelper testHelper = new DefinedOverlayTestHelper();


        [SetUp]
        public void SetUp()
        {
            testHelper.SetUp(new CompanyNameOverlayImpl(null));
        }

        [Test]
        public void TestDefinedTag()
        {
            testHelper.TestDefinedTag("[[COMPANY_NAME]]");
        }

        [Test]
        public void TestOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetCompanyName().Returns(expectedRequestValue); return true; });
        }

    }
}
