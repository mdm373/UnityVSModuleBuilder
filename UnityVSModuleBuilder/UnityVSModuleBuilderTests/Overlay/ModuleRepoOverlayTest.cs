using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    class ModuleRepoOverlayTest
    {
        private readonly DefinedOverlayTestHelper testHelper = new DefinedOverlayTestHelper();

        [SetUp]
        public void SetUp()
        {
            testHelper.SetUp(new ModuleRepoOverlay(null));
        }

        [Test]
        public void TestDefinedTag()
        {
            testHelper.TestDefinedTag("[[MODULE_REPO]]");
        }

        [Test]
        public void TestOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetModuleRepositoryLocation().Returns(expectedRequestValue); return true; });
        }
    }
}
