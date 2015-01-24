using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    public class ModuleRepoOverlay : DefinedOverlayImpl
    {
        public ModuleRepoOverlay(FileSystemController fs) : base(fs) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetModuleRepositoryLocation();
        }

        public override string GetDefinedTag()
        {
            return OverlayConstants.MODULE_REPO_DEFINED_TAG;
        }
    }
}
