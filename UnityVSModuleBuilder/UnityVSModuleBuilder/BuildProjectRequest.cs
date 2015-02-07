using System;

namespace UnityVSModuleBuilder
{
    public interface BuildProjectRequest
    {
        String GetProjectName();
        String GetCopyLocation();
        String GetCompanyName();
        String GetCompanyShortName();
        String GetUnityLocation();
    }
}
