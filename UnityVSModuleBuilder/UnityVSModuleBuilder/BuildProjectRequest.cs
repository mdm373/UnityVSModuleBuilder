using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder
{
    public interface BuildProjectRequest
    {
        String GetProjectName();
        String GetCopyLocation();
        String GetCompanyName();
        String GetCompanyShortName();
        String GetUnityLocation();
        String GetModuleRepositoryLocation();
    }
}
