using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityVSModuleEditor.UnityApis;
using UnityVSModuleEditor.XMLStore;

namespace UnityVSModuleEditor.MiddleTier
{
    public class VSModuleFactory
    {
        public static VSModuleDelegate GetDelegate()
        {
            UnityApi unityApi = new UnityApiImpl();
            return new VSModuleDelegate(unityApi, new VSModuleXmlSerializer(unityApi));
        }
    }
}
