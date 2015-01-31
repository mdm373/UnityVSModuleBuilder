using NSubstitute;
using NUnit.Framework;

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
        public void TestUnityLocationDefinedTag()
        {
            testHelper.TestDefinedTag("[[UNITY_LOCATION]]");
        }

        [Test]
        public void TestUnityLocationOverlayValue()
        {
            testHelper.TestOverlayValue((request, expectedRequestValue) => { request.GetUnityLocation().Returns(expectedRequestValue); return true; });
        }
    }
}
