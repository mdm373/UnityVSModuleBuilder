using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Overlay
{
    public class ProjectNameOverlay : DefinedOverlayImpl
    {
        private const string PROJECT_NAME_TAG = "[[PROJECT_NAME]]";

        public ProjectNameOverlay(FileSystem.FileSystemController fileSystem) : base(fileSystem) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetProjectName();
        }

        public override string GetDefinedTag()
        {
            return PROJECT_NAME_TAG;
        }
    }
}
