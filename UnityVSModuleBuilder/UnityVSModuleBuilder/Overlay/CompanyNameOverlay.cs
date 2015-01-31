using UnityVSModuleCommon.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    internal class CompanyNameOverlayImpl : DefinedOverlayImpl
    {
        public CompanyNameOverlayImpl(FileSystemController fileSystem) : base(fileSystem) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetCompanyName();
        }

        public override string GetDefinedTag()
        {
            return OverlayConstants.COMPANY_NAME_DEFINED_TAG;
        }
    }
}
