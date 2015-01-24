using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleBuilder.FileSystem;

namespace UnityVSModuleBuilder.Overlay
{
    public class CompanyShortNameOverlay : DefinedOverlayImpl
    {
        public CompanyShortNameOverlay(FileSystemController fs) : base(fs) { }

        public override string GetDefinedValue(BuildProjectRequest request)
        {
            return request.GetCompanyShortName();
        }

        public override string GetDefinedTag()
        {
            return OverlayConstants.COMPANY_SHORT_NAME_DEFINED_TAG;
        }
    }
}
