using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSubstitute;

namespace UnityVSModuleBuilder.Overlay
{
    [TestFixture]
    public class UnityLocationOverlayTest 
    {
        private readonly DefinedOverlayTestHelper testHelper = new DefinedOverlayTestHelper();

        [SetUp]
        public void SetUp()
        {
            testHelper.SetUp(new UnityLocationOverlay(null));
        }

        [Test]
        public void TestDefinedTag()
        {
            testHelper.TestDefinedTag("[[UNITY_LOCATION]]");
        }

        [Test]
        public void TestOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetUnityLocation().Returns(expectedRequestValue); return true; });
        }
    }
}
