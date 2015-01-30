using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityVSModuleEditor.UnityApis
{
    internal class UnityApiFactory
    {
        private UnityApiFactory()
        {

        }

        public static UnityApi GetUnityApi()
        {
            return new UnityApiImpl();
        }
    }
}
