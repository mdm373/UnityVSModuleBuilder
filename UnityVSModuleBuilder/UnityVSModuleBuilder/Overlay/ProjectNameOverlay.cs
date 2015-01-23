using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleBuilder.Overlay
{
    public interface ProjectNameOverlay
    {
        bool Overlay(string projectName, string projectLocation);
        
    }
}
