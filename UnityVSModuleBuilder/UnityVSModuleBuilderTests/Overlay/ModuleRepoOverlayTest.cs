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
        public void TestModuleRepositoryDefinedTag()
        {
            testHelper.TestDefinedTag("[[MODULE_REPO]]");
        }

        [Test]
        public void TestModuleRepositoryOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetModuleRepositoryLocation().Returns(expectedRequestValue); return true; });
        }
    }
}
