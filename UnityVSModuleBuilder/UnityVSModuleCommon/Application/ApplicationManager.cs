using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.Application
{
    public interface ApplicationManager
    {
        ApplicationSettingsResponse RetrieveApplicationSettings();
        void SaveApplicationSettings(ApplicationSettings settings);
    }
}
