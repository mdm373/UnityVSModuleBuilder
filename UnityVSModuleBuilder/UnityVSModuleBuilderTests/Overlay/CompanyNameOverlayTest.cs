﻿using NSubstitute;
using NUnit.Framework;

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
        public void TestCompanyNameDefinedTag()
        {
            testHelper.TestDefinedTag("[[COMPANY_NAME]]");
        }

        [Test]
        public void TestCompanyNameOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetCompanyName().Returns(expectedRequestValue); return true; });
        }

    }
}
