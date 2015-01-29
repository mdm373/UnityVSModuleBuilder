using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Overlay
{
    class DefinedOverlayTestHelper
    {
        private const string EXPECTED_REQUEST_VALUE = "EXPECTED_REQUEST_VALUE";

        private DefinedOverlayImpl definedOverlay;
        private BuildProjectRequest request;
        private string value;
        
        public void SetUp(DefinedOverlayImpl definedOverlay)
        {
            this.definedOverlay = definedOverlay;
        }

        
        public void TestDefinedTag(String expectedDefinedTag)
        {
            Assert.AreEqual(expectedDefinedTag, definedOverlay.GetDefinedTag());
        }

        
        
        public void TestOverlayValue(Func<BuildProjectRequest, String, bool> requestAssignment)
        {
            GivenRequestWithProvidedValue(requestAssignment);
            WhenGettingOverlayValue();
            ThenOverlayValueIsProvidedValue();
        }

        
        private void GivenRequestWithProvidedValue(Func<BuildProjectRequest, String, bool> requestAssignment)
        {
            this.request = Substitute.For<BuildProjectRequest>();
            requestAssignment(request, EXPECTED_REQUEST_VALUE);
        }

        private void WhenGettingOverlayValue()
        {
            this.value = definedOverlay.GetDefinedValue(request);
        }

        private void ThenOverlayValueIsProvidedValue()
        {
            Assert.AreEqual(EXPECTED_REQUEST_VALUE, value);
        }

    }
}
