using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    internal class ModuleRepoOverlay : DefinedOverlayImpl
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
