using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder
{
    public interface TemplateProjectBuilder
    {
        BuildProjectResponse DoBuild(BuildProjectRequest request);
    }
}
