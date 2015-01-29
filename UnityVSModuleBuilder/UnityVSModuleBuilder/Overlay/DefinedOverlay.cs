using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Overlay
{
    internal interface DefinedOverlay
    {
        bool Overlay(BuildProjectRequest request);
        
    }
}
