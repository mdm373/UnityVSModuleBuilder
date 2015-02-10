using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.TemplateCopy
{
    internal interface TemplateCopyController
    {
        bool CopyAndCleanTemplate(string copyLocation, string projectName);
    }
}
