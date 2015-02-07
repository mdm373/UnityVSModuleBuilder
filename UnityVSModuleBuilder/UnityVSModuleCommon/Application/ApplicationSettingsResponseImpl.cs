using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.Application
{

    public enum AppSettingsCode
    {
        SUCCESS, INSTALL_NOT_FOUND, UNKNOWN_ERROR
    }

    class ApplicationSettingsResponseImpl : ApplicationSettingsResponse
    {
        private readonly ApplicationSettings settings;
        private readonly AppSettingsCode code;

        public ApplicationSettingsResponseImpl(ApplicationSettings settings, AppSettingsCode code)
        {
            this.settings = settings;
            this.code = code;
        }

        public ApplicationSettings GetApplicationSettings()
        {
            return settings;
        }


        public AppSettingsCode GetCode()
        {
            return code;
        }
    }
}
