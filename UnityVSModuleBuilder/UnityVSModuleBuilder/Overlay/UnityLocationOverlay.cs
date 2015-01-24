using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    public class UnityLocationOverlay : DefinedOverlayImpl
    {
        public UnityLocationOverlay(FileSystemController fs) : base(fs) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetUnityLocation();
        }

        public override string GetDefinedTag()
        {
            return OverlayConstants.UNITY_LOCATION_DEFINED_TAG;
        }
    }
}
