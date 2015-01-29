using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    internal class ProjectNameOverlay : DefinedOverlayImpl
    {
        public ProjectNameOverlay(FileSystemController fileSystem) : base(fileSystem) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetProjectName();
        }

        public override string GetDefinedTag()
        {
            return OverlayConstants.PROJECT_NAME_TAG;
        }
    }
}
