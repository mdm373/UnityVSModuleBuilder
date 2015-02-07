using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleCommon.Application
{
    internal class ApplicationSettingsImpl : ApplicationSettings
    {
        private readonly string repoLocation;

        public ApplicationSettingsImpl(string repoLocation)
        {
            this.repoLocation = repoLocation;
        }

        public string GetRepoLocation()
        {
            return repoLocation;
        }
    }
}
