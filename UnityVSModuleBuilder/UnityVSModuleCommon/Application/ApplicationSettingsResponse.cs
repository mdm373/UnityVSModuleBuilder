using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.Application
{
    public interface ApplicationSettingsResponse
    {
        ApplicationSettings GetApplicationSettings();

        AppSettingsCode GetCode();
    }
}
